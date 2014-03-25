using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionPostponed
	{
		public string TransmissionId { get; set; }
		public DateTime NewStart { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been postponed to {1:O}", TransmissionId, NewStart);
		}
	}
}