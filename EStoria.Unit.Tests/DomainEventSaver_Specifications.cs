using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;

namespace EStoria.Unit.Tests
{
	public class DomainEventSaverSetup : WithSubject<DomainEventSaver>
	{
		protected static List<CommittedEvent> Events;
		protected static Subject<IDomainEvent> DomainEvents;
		protected static TestDomainEvent SomeDomainEvent;
		protected static Exception Exception;

		Establish context = () =>
		{
			Events = new List<CommittedEvent>();
			DomainEvents = new Subject<IDomainEvent>();
			SomeDomainEvent = new TestDomainEvent { AggregateId = "aggregateId", Value = "some value" };

			Configure(registrar => registrar.For<IObservable<IDomainEvent>>().Use(DomainEvents));
			The<IEventStore>().WhenToldTo(store => store.Append(Arg.Any<string>(), Arg.Any<object>())).Return<string, object>((s, o) => new CommittedEvent(1, s, DateTime.Now, o));

			Subject.Subscribe(Events.Add, ex => Exception = ex);
		};
	}

	[Subject(typeof(DomainEventSaver))]
	public class When_receiving_domain_events : DomainEventSaverSetup
	{			
		Because of = () => DomainEvents.OnNext(SomeDomainEvent);

		It should_save_the_event_on_the_store = () => The<IEventStore>().WasToldTo(store => store.Append("aggregateId", SomeDomainEvent)).OnlyOnce();
		It should_publish_the_committed_event_corresponding_to_the_event = () => Events.Select(evt => evt.Data).Should().ContainInOrder(SomeDomainEvent);
	}

	[Subject(typeof(DomainEventSaver))]
	public class When_failing_to_append_the_events_to_the_store : DomainEventSaverSetup
	{
		Establish context = () => The<IEventStore>().WhenToldTo(store => store.Append(Arg.Any<string>(), Arg.Any<object>())).Throw(new Exception("some exception"));

		Because of = () => DomainEvents.OnNext(SomeDomainEvent);

		It should_not_publish_the_committed_events = () => Events.Should().BeEmpty();
		It should_publish_the_exception = () => Exception.Should().NotBeNull();
	}
}