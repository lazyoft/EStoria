using System;

namespace EStoria.Sample.Domain.Events
{
	public class TransmissionCanceled
	{
		public string TransmissionId { get; set; }

		public override string ToString()
		{
			return string.Format("Transmission {0} has been canceled", TransmissionId);
		}
	}
}