using System;

namespace EStoria.Sample.Domain.Aggregates
{
	public class Transmission
	{
		public string Id { get; set; }
		public string ChannelId { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public bool Canceled { get; set; }
	}
}