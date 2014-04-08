using System;
using EStoria.Sample.Domain.Events;
using EStoria.Sample.Readmodels;
using EStoria.ValueObjects;

namespace EStoria.Sample.Projections
{
	public class ChannelStatisticsProjection : EventModel<ChannelStatistics>
	{
		public ChannelStatisticsProjection(IObservable<CommittedEvent> events,
			ChannelStatistics modelSnapshot = null, int serial = 0) : base(events, modelSnapshot, serial)
		{
			HandleEvents(configuration => configuration
				.When<TransmissionScheduled>((stats, scheduled) => stats.AddTransmission(scheduled.TransmissionId, scheduled.ChannelId))
				.When<TransmissionCanceled>((stats, canceled) => stats[canceled.TransmissionId].Canceled++)
				.When<TransmissionDeleted>((stats, deleted) => stats[deleted.TransmissionId].Removed++)
				.When<TransmissionBroughtForward>((stats, forward) => stats[forward.TransmissionId].ScheduleChanges++)
				.When<TransmissionExtended>((stats, extended) => stats[extended.TransmissionId].ScheduleChanges++)
				.When<TransmissionMoved>((stats, moved) => stats[moved.TransmissionId].ScheduleChanges++)
				.When<TransmissionPostponed>((stats, postponed) => stats[postponed.TransmissionId].ScheduleChanges++)
				.When<TransmissionShortened>((stats, shortened) => stats[shortened.TransmissionId].ScheduleChanges++)
			);
		}
	}
}