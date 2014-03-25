using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionExtended
	{
		public string TransmissionId { get; set; }
		public TimeSpan NewDuration { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been extended with a duration of {1}", TransmissionId, NewDuration);
		}
	}
}