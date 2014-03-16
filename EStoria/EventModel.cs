using System;
using System.Collections.Generic;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public abstract class EventModel<TModel> : BaseDisposable where TModel : class, new()
	{
		public TModel Model { get; private set; }
		public int Serial { get; private set; }

		protected IEventHandlingConfiguration<TModel> ConfigureEvents { get; private set; }
		readonly IDisposable _subscription;
		bool _disposed;
		
		protected EventModel(IObservable<CommittedEvent> events, TState snapshot = null, int serial = 0)
		internal readonly Dictionary<Type, object> Handlers;
		internal Action<TModel, object> UnknownHandler;

		protected EventModel(IObservable<CommittedEvent> events, TModel modelSnapshot = null, int serial = 0)
		{
			Guard.NotNull(() => events);

			Handlers = new Dictionary<Type, object>();
			UnknownHandler = (_, __) => { };
			Model = modelSnapshot ?? new TModel();
			Serial = serial;

			// ReSharper disable once DoNotCallOverridableMethodsInConstructor
			Configure();
			_subscription = events.Subscribe(ApplyEvent);
		}

		protected abstract void Configure();

		void ApplyEvent(CommittedEvent evt)
		{
			Guard.NotNull(() => evt);

			if(evt.Serial <= Serial)
				return;

			var handler = Handlers.ContainsKey(evt.Data.GetType()) ? Handlers[evt.Data.GetType()] : UnknownHandler;
			handler.GetType().GetMethod("Invoke").Invoke(handler, new[] { Model, evt.Data });

			Serial = evt.Serial;
		}

		protected override void Dispose(bool disposing)
		{
			if(!Disposed && disposing)
				_subscription.Dispose();
			base.Dispose(disposing);
		}
	}
}