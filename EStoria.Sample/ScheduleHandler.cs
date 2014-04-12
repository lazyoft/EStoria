using System;
using System.Collections.Generic;
using System.Linq;
using EStoria.Interfaces;
using EStoria.Sample.Domain.Aggregates;
using EStoria.Sample.Domain.Commands;
using EStoria.Sample.Domain.Events;
using EStoria.ValueObjects;

namespace EStoria.Sample
{
	public class ScheduleHandler : CommandHandler, IHandlesCommand<ScheduleTransmission>, IHandlesCommand<CancelTransmission>, IHandlesCommand<DeleteTransmission>
	{
		readonly IModelLoader _loader;

		public ScheduleHandler(IModelLoader loader)
		{
			_loader = loader;
		}

		public IEnumerable<DomainEvent> Apply(ScheduleTransmission command)
		{
			var channels = _loader.Load<ScheduleAggregate>("channels");
			if(!channels.Model.ContainsKey(command.TransmissionId))
			{
				/*     s                          e
				 *     |--------------------------|
				 *                      S                E
				 *                      |----------------| 
				 *           S           E
				 *           |-----------|
				 *   S               E
				 *   |---------------|
				 * S                                   E
				 * |-----------------------------------|
				 * 
				 * ==> S < e && E > s
				 */
				if (channels.Model.Values.Any(t => t.ChannelId == command.ChannelId && t.Id != command.TransmissionId && (command.Start < t.To && command.Start.Add(command.Duration) > t.From)))
				{
					FailCommand(command, "Transmission has an overlap");
					yield break;
				}
				yield return For("channels").A(new TransmissionScheduled
				{
					ChannelId = command.ChannelId,
					TransmissionId = command.TransmissionId,
					From = command.Start,
					To = command.Start.Add(command.Duration)
				});
			}
			else
			{
				var transmission = channels.Model[command.TransmissionId];
				if(transmission.Canceled)
				{
					FailCommand(command, "Transmission is canceled");
					yield break;
				}
				if (transmission.ChannelId != command.ChannelId)
					yield return For("channels").A(new TransmissionMoved { TransmissionId = transmission.Id, FromChannelId = transmission.ChannelId, ToChannelId = command.ChannelId });
				if (command.Start < transmission.From)
					yield return For("channels").A(new TransmissionBroughtForward { TransmissionId = transmission.Id, NewStart = command.Start });
				if (command.Start > transmission.From)
					yield return For("channels").A(new TransmissionPostponed { TransmissionId = transmission.Id, NewStart = command.Start });
				if (command.Duration < transmission.To - transmission.From)
					yield return For("channels").A(new TransmissionShortened { TransmissionId = transmission.Id, Duration = command.Duration });
				if (command.Duration > transmission.To - transmission.From)
					yield return For("channels").A(new TransmissionExtended { TransmissionId = transmission.Id, NewDuration = command.Duration });				
			}
		}

		public IEnumerable<DomainEvent> Apply(CancelTransmission command)
		{
			var channels = _loader.Load<ScheduleAggregate>("channels");

			if(!channels.Model.ContainsKey(command.TransmissionId))
			{
				FailCommand(command, "Transmission does not exist");
				yield break;
			}
			if(channels.Model[command.TransmissionId].Canceled)
			{
				FailCommand(command, "Transmission already canceled");
				yield break;
			}

			yield return For("channels").A(new TransmissionCanceled { TransmissionId = command.TransmissionId });
		}

		public IEnumerable<DomainEvent> Apply(DeleteTransmission command)
		{
			var channels = _loader.Load<ScheduleAggregate>("channels");
			if(!channels.Model.ContainsKey(command.TransmissionId))
			{
				FailCommand(command, "Transmission does not exist");
				yield break;
			}
			yield return For("channels").A(new TransmissionDeleted { TransmissionId = command.TransmissionId });
		}
	}
}
