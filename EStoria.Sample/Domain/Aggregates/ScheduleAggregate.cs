using System;
using System.Collections.Generic;
using EStoria.Sample.Domain.Events;
using EStoria.ValueObjects;

namespace EStoria.Sample.Domain.Aggregates
{
	public class ScheduleAggregate : EventModel<Dictionary<string, Transmission>>
	{
		public ScheduleAggregate(IObservable<CommittedEvent> events, Dictionary<string, Transmission> modelSnapshot = null,
			int serial = 0) : base(events, modelSnapshot, serial)
		{
			HandleEvents(config => config
				.When<TransmissionScheduled>((transmission, @event) =>
					transmission[@event.TransmissionId] = new Transmission
					{
						Id = @event.TransmissionId, 
						ChannelId = @event.ChannelId, 
						From = @event.From, 
						To = @event.To
					})
				.When<TransmissionCanceled>((transmission, @event) => transmission[@event.TransmissionId].Canceled = true)
				.When<TransmissionDeleted>((transmission, @event) => transmission.Remove(@event.TransmissionId))
				.When<TransmissionExtended>((transmission, @event) => transmission[@event.TransmissionId].To = transmission[@event.TransmissionId].From + @event.NewDuration)
				.When<TransmissionShortened>((transmission, @event) => transmission[@event.TransmissionId].To = transmission[@event.TransmissionId].From + @event.Duration)
				.When<TransmissionBroughtForward>((transmission, @event) => transmission[@event.TransmissionId].From = @event.NewStart)
				.When<TransmissionPostponed>((transmission, @event) => transmission[@event.TransmissionId].From = @event.NewStart)
				.When<TransmissionMoved>((transmission, @event) => transmission[@event.TransmissionId].ChannelId = @event.ToChannelId)
			);
		}
	}
}