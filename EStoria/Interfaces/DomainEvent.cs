using System;

namespace EStoria.Interfaces
{
	public class DomainEvent
	{
		public string AggregateId { get; set; }
		public object Event { get; set; }
	}
}