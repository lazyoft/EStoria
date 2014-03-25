using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionMoved
	{
		public string TransmissionId { get; set; }
		public string FromChannelId { get; set; }
		public string ToChannelId { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been moved from channel {1} to channel {2}", TransmissionId, FromChannelId, ToChannelId);
		}
	}
}