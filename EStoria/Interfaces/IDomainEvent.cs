using System;

namespace EStoria.Interfaces
{
	public interface IDomainEvent
	{
		string AggregateId { get; set; }
	}
}