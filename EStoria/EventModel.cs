using System;
using System.Collections.Generic;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public abstract class EventModel<TModel> : BaseDisposable, IEventModel where TModel : class, new()
	{
		public TModel Model { get; private set; }
		public int Serial { get; private set; }

		internal readonly Dictionary<Type, object> Handlers;
		internal Action<TModel, object> UnknownHandler;

		readonly IObservable<CommittedEvent> _events;
		IDisposable _subscription;

		protected EventModel(IObservable<CommittedEvent> events, TModel modelSnapshot = null, int serial = 0)
		{
			Guard.NotNull(() => events);

			Handlers = new Dictionary<Type, object>();
			UnknownHandler = (_, __) => { };
			Model = modelSnapshot ?? new TModel();
			Serial = serial;
			_events = events;
		}

		public void HandleEvents(Action<IEventHandlingConfiguration<TModel>> eventStream)
		{
			Guard.NotNull(() => eventStream);

			eventStream(new EventHandlingConfiguration<TModel>(this));
			Start();
		}

		void Start()
		{
			if(_subscription == null)
				_subscription = _events.Subscribe(ApplyEvent);
		}

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