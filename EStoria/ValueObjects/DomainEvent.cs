using System;

namespace EStoria.ValueObjects
{
	public class DomainEvent
	{
		public string AggregateId { get; set; }
		public object Event { get; set; }
	}
}