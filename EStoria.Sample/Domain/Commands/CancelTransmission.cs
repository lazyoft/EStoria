using System;
using EStoria.Interfaces;

namespace EStoria.Sample.Domain.Commands
{
	public class CancelTransmission : ICommand
	{
		public string TransmissionId { get; set; }

		public override string ToString()
		{
			return string.Format("Cancel transmission {0}", TransmissionId);
		}
	}
}