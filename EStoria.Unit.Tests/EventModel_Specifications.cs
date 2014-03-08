using System;
using System.Reactive.Subjects;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	public class EventModelSetup : WithSubject<TestEventModel>
	{
		protected static ISubject<CommittedEvent> Events;

		Establish context = () =>
		{
			Events = new Subject<CommittedEvent>();
			Configure(registrar => registrar.For<IObservable<CommittedEvent>>().Use(Events));
		};
	}

	[Subject(typeof(EventModel<>))]
	public class When_receiving_events : EventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, "text"));
			Events.OnNext(new CommittedEvent(2, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(3, "test", DateTime.Now, new DateTime(1971, 11, 26, 12, 30, 00)));
			Events.OnNext(new CommittedEvent(4, "test", DateTime.Now, 22));
		};

		It should_apply_the_events_to_the_state = () =>
		{
			Subject.State.Text.Should().Be("text");
			Subject.State.Number.Should().Be(42);
			Subject.State.Date.Should().Be(new DateTime(1971, 11, 26, 12, 30, 00));
		};
	}

	[Subject(typeof(EventModel<>))]
	public class When_receiving_duplicate_events : EventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
		};

		It should_ignore_the_duplicated_events_and_apply_the_event_just_once = () => Subject.State.Number.Should().Be(20);
	}

	[Subject(typeof(EventModel<>))]
	public class When_receiving_out_of_order_events : EventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(2, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(3, "test", DateTime.Now, 22));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
		};

		It should_ignore_the_out_of_order_events_and_apply_only_the_events_in_sequence = () => Subject.State.Number.Should().Be(42);
	}
}
