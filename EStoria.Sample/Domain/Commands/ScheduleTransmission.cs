using System;
using EStoria.Interfaces;

namespace EStoria.Sample.Domain.Commands
{
	public class ScheduleTransmission : ICommand
	{
		public string ChannelId { get; set; }
		public string TransmissionId { get; set; }
		public DateTime Start { get; set; }
		public TimeSpan Duration { get; set; }

		public override string ToString()
		{
			return string.Format("Schedule transmission {0} on channel {1} starting at {2:O} with duration {3}", TransmissionId,
				ChannelId, Start, Duration);
		}
	}
}