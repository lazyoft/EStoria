using System;
using System.Collections.Generic;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public class EventStore : IEventStore
	{
		readonly IEventPersistence _persistence;
		readonly ISerialProvider _serialProvider;
		readonly IClock _clock;
		readonly ISerializer _serializer;

		public EventStore(IEventPersistence persistence, ISerialProvider serialProvider, IClock clock, ISerializer serializer)
		{			
			Guard.NotNull(() => persistence);
			Guard.NotNull(() => serialProvider);
			Guard.NotNull(() => clock);
			Guard.NotNull(() => serializer);

			_persistence = persistence;
			_serialProvider = serialProvider;
			_clock = clock;
			_serializer = serializer;
		}

		public IEnumerable<CommittedEvent> Append(string streamName, IEnumerable<object> events)
		{
			Guard.NotNull(() => events);
			Guard.NotNullOrWhiteSpace(() => streamName);
			Guard.DoesntContainNull(() => events);

			var result = new List<CommittedEvent>();
			foreach(var @event in events)
			{
				var serial = _serialProvider.Next();
				var savedEvent = new CommittedEvent(serial, streamName, _clock.Now(), @event);
				_persistence.Save(new CommitInfo(serial, streamName), _serializer.Serialize(savedEvent));
				result.Add(savedEvent);
			}
			return result;
		}

		public CommittedEvent Append(string streamName, object @event)
		{
			return Append(streamName, Enumerable.Repeat(@event, 1)).First();
		}

		public IEnumerable<CommittedEvent> Read(string streamName = null, int startWithSerial = 0)
		{
			return from info in _persistence.GetInfos() 
				   where (string.IsNullOrEmpty(streamName) || streamName == info.Name) && info.Serial >= startWithSerial
				   select _serializer.Deserialize<CommittedEvent>(_persistence.Read(info));
		}
	}
}