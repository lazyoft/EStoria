using System;
using EStoria.Interfaces;

namespace EStoria.Sample.Domain.Commands
{
	public class DeleteTransmission : ICommand
	{
		public string TransmissionId { get; set; }

		public override string ToString()
		{
			return string.Format("Delete transmission {0}", TransmissionId);
		}
	}
}