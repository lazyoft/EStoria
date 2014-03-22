using System;
using System.Reactive.Subjects;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public class DomainEventSaver : BaseDisposable, IObservable<CommittedEvent>
	{
		readonly Subject<CommittedEvent> _committedEvents;
		readonly IDisposable _subscription;

		public DomainEventSaver(IObservable<DomainEvent> domainEvents, IEventStore eventStore)
		{
			_committedEvents = new Subject<CommittedEvent>();
			_subscription = domainEvents.Subscribe(evt =>
			{
				var commit = default(CommittedEvent);
				try
				{
					commit = eventStore.Append(evt.AggregateId, evt.Event);
				}
				catch(Exception ex)
				{
					_committedEvents.OnError(ex);
				}
				_committedEvents.OnNext(commit);
			});
		}

		public IDisposable Subscribe(IObserver<CommittedEvent> observer)
		{
			return _committedEvents.Subscribe(observer);
		}

		protected override void Dispose(bool disposing)
		{
			if(!Disposed && disposing)
			{
				_subscription.Dispose();
				_committedEvents.OnCompleted();
				_committedEvents.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}