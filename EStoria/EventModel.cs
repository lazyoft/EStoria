using System;
using System.Collections.Generic;
using EStoria.ValueObjects;

namespace EStoria
{
	public abstract class EventModel<TState> : IDisposable where TState : class, new()
	{
		public TState State { get; private set; }
		public int Serial { get; private set; }
		protected EventModelConfiguration Apply { get; private set; } 

		readonly IDisposable _subscription;
		readonly Dictionary<Type, object> Handlers;
		bool _disposed;
		
		protected EventModel(IObservable<CommittedEvent> events, TState snapshot = null, int serial = 0)
		{
			Guard.NotNull(() => events);

			Handlers = new Dictionary<Type, object>();
			Apply = new EventModelConfiguration(this);
			State = snapshot ?? new TState();
			Serial = serial;

			// ReSharper disable once DoNotCallOverridableMethodsInConstructor
			Configure();
			_subscription = events.Subscribe(ApplyEvent);
		}

		protected abstract void Configure();

		void ApplyEvent(CommittedEvent evt)
		{
			Guard.NotNull(() => evt);

			if(evt.Serial <= Serial || !Handlers.ContainsKey(evt.Data.GetType())) 
				return;
			var handler = Handlers[evt.Data.GetType()];
			handler.GetType().GetMethod("Invoke").Invoke(handler, new[] { State, evt.Data });

			Serial = evt.Serial;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
				_subscription.Dispose();
			_disposed = true;
		}

		public sealed class EventModelConfiguration
		{
			readonly EventModel<TState> _eventModel;

			public EventModelConfiguration(EventModel<TState> eventModel)
			{
				_eventModel = eventModel;
			}

			public EventModelConfiguration When<T>(Action<TState, T> handler)
			{
				_eventModel.Handlers[typeof(T)] = handler;
				return this;
			}
		}
	}
}