using System;
using EStoria.Interfaces;

namespace EStoria
{
	class DomainEventBuilder : CommandHandler.IDomainEventBuilder
	{
		readonly string _aggregateId;

		public DomainEventBuilder(string aggregateId)
		{
			_aggregateId = aggregateId;
		}

		public DomainEvent A(object @event)
		{
			return new DomainEvent { AggregateId = _aggregateId, Event = @event };
		}
	}
}