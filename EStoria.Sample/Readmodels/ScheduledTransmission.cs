using System;

namespace EStoria.Sample.Readmodels
{
	public class ScheduledTransmission
	{
		public string ChannelId { get; set; }
		public string TransmissionId { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public TimeSpan Duration { get { return End - Start; } }
	}
}