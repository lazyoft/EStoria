using System;
using System.Reactive.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public class ModelLoader : IModelLoader
	{
		readonly IEventStore _eventStore;
		readonly ISnapshotStore _snapshotStore;
		readonly IObservable<CommittedEvent> _events;

		public ModelLoader(IEventStore eventStore, ISnapshotStore snapshotStore, IObservable<CommittedEvent> events)
		{
			Guard.NotNull(() => eventStore);
			Guard.NotNull(() => snapshotStore);
			Guard.NotNull(() => events);

			_eventStore = eventStore;
			_snapshotStore = snapshotStore;
			_events = events;
		}

		public T Load<T>(string id) where T : IEventModel
		{
			Guard.NotNullOrWhiteSpace(() => id);

			var snapshot = _snapshotStore.Read(id);
			return (T)Activator.CreateInstance(typeof(T), _events.StartWith(_eventStore.Read(id, snapshot.Serial)), snapshot.Data, snapshot.Serial);
		}
	}
}