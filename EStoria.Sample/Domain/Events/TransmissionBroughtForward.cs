using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionBroughtForward
	{
		public string TransmissionId { get; set; }
		public DateTime NewStart { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been brought forward to {1:O}", TransmissionId, NewStart);
		}
	}
}