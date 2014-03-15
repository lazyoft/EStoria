using System;

namespace EStoria.Interfaces
{
	public interface IPublisher<in T>
	{
		void Publish(T message);
	}
}