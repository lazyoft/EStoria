using System;
using EStoria.Sample.Domain.Aggregates;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionScheduled
	{
		public string ChannelId { get; set; }
		public string TransmissionId { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been scheduled on channel {1} from {2:O} to {3:O}", TransmissionId,
				ChannelId, From, To);
		}
	}
}