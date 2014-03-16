using System;

namespace EStoria.Interfaces
{
	public interface IEventHandlingConfiguration<out TState>
	{
		IEventHandlingConfiguration<TState> When<T>(Action<TState, T> handler);
		void WhenUnknown(Action<TState, object> unknownHandler);
	}
}