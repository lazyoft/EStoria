using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using EStoria.Interfaces;

namespace EStoria.Services
{
	public class MessageBus : IMessageBus
	{
		readonly ConcurrentDictionary<Type, object> Subjects = new ConcurrentDictionary<Type, object>();

		class AnonymousPublisher<T> : IPublisher<T>
		{
			readonly Action<T> InternalPublish;

			public AnonymousPublisher(Action<T> publish)
			{
				InternalPublish = publish;
			}

			public void Publish(T message)
			{
				InternalPublish(message);
			}
		}

		void Publish<T>(T message)
		{
			var subjects = Subjects.Where(pair => pair.Key.IsInstanceOfType(message)).Select(pair => pair.Value);

			// We have to duck type the subject as ISubject<T> could not be covariant
			foreach (var subject in subjects)
				subject.GetType().GetMethod("OnNext").Invoke(subject, new object[] { message });
		}

		public IObservable<T> AsObservable<T>()
		{
			var subject = (ISubject<T>)Subjects.GetOrAdd(typeof(T), t => new Subject<T>());
			return subject.AsObservable();
		}

		public IPublisher<T> AsPublisher<T>()
		{
			return new AnonymousPublisher<T>(Publish);
		}
	}
}