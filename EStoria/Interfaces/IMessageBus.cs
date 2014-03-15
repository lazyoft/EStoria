using System;

namespace EStoria.Interfaces
{
	public interface IMessageBus
	{
		IObservable<T> AsObservable<T>();
		IPublisher<T> AsPublisher<T>();
	}
}