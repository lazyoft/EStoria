using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionShortened
	{
		public string TransmissionId { get; set; }
		public TimeSpan Duration { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been shortened to a duration of {1}", TransmissionId, Duration);
		}
	}
}