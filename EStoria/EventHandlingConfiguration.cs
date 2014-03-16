using System;
using EStoria.Interfaces;

namespace EStoria
{
	sealed class EventHandlingConfiguration<TState> : IEventHandlingConfiguration<TState> where TState : class, new()
	{
		readonly EventModel<TState> _eventModel;

		public EventHandlingConfiguration(EventModel<TState> eventModel)
		{
			_eventModel = eventModel;
		}

		public IEventHandlingConfiguration<TState> When<T>(Action<TState, T> handler)
		{
			Guard.NotNull(() => handler);

			_eventModel.Handlers[typeof(T)] = handler;
			return this;
		}

		public void WhenUnknown(Action<TState, object> unknownHandler)
		{
			Guard.NotNull(() => unknownHandler);

			_eventModel.UnknownHandler += unknownHandler;
		}
	}
}