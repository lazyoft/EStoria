using System;
using EStoria.Sample.Domain.Events;
using EStoria.Sample.Readmodels;
using EStoria.ValueObjects;

namespace EStoria.Sample.Projections
{
	public class ScheduleByDayProjection : EventModel<ScheduleByDay>
	{
		public ScheduleByDayProjection(IObservable<CommittedEvent> events, ScheduleByDay modelSnapshot = null, int serial = 0)
			: base(events, modelSnapshot, serial)
		{
			HandleEvents(configuration => configuration
				.When<TransmissionScheduled>((day, scheduled) => day[scheduled.TransmissionId] = new ScheduledTransmission
				{
					ChannelId = scheduled.ChannelId, 
					TransmissionId = scheduled.TransmissionId, 
					Start = scheduled.From, 
					End = scheduled.To
				})
				.When<TransmissionCanceled>((day, canceled) => day.Remove(canceled.TransmissionId))
				.When<TransmissionDeleted>((day, deleted) => day.Remove(deleted.TransmissionId))
				.When<TransmissionBroughtForward>((day, forward) => day[forward.TransmissionId].Start = forward.NewStart)
				.When<TransmissionExtended>((day, extended) => day[extended.TransmissionId].End = day[extended.TransmissionId].Start + extended.NewDuration)
				.When<TransmissionMoved>((day, moved) => day[moved.TransmissionId].ChannelId = moved.ToChannelId)
				.When<TransmissionPostponed>((day, postponed) => day[postponed.TransmissionId].Start = postponed.NewStart)
				.When<TransmissionShortened>((day, shortened) => day[shortened.TransmissionId].End = day[shortened.TransmissionId].Start + shortened.Duration)
			);
		}
	}
}
