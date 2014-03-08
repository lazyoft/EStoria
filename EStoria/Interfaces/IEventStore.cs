using System;
using System.Collections.Generic;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface IEventStore
	{
		IEnumerable<CommittedEvent> Append(string streamName, IEnumerable<object> events);
		CommittedEvent Append(string streamName, object @event);

		IEnumerable<CommittedEvent> Read(string streamName = null, int startWithSerial = 0);
	}
}