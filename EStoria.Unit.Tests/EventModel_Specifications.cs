using System;
using System.Reactive.Subjects;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	public class EmptyEventModelSetup : WithSubject<TestEventModel>
	{
		protected static ISubject<CommittedEvent> Events;

		Establish context = () =>
		{
			Events = new Subject<CommittedEvent>();
			Configure(registrar => registrar.For<IObservable<CommittedEvent>>().Use(Events));
		};
	}

	[Subject(typeof(EventModel<>), "empty")]
	public class When_receiving_events : EmptyEventModelSetup
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
		It should_update_its_serial_accordingly = () => Subject.Serial.Should().Be(4);
	}

	[Subject(typeof(EventModel<>), "empty")]
	public class When_receiving_duplicate_events : EmptyEventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
		};

		It should_ignore_the_duplicated_events_and_apply_the_event_just_once = () => Subject.State.Number.Should().Be(20);
		It should_update_its_serial_accordingly = () => Subject.Serial.Should().Be(1);
	}

	[Subject(typeof(EventModel<>), "empty")]
	public class When_receiving_out_of_order_events : EmptyEventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(2, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(3, "test", DateTime.Now, 22));
			Events.OnNext(new CommittedEvent(1, "test", DateTime.Now, 20));
		};

		It should_ignore_the_out_of_order_events_and_apply_only_the_events_in_sequence = () => Subject.State.Number.Should().Be(42);
		It should_update_its_serial_accordingly = () => Subject.Serial.Should().Be(3);
	}

	public class SnapshotEventModelSetup
	{
		protected static ISubject<CommittedEvent> Events;
		protected static TestEventModel Subject;

		Establish context = () =>
		{
			Events = new Subject<CommittedEvent>();
			Subject = new TestEventModel(Events, new TestModel { Number = 42 }, 10);
		};
	}

	[Subject(typeof(EventModel<>), "initialized with a snapshot")]
	public class When_receiving_events_occurred_before_its_serial : SnapshotEventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(9, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(10, "test", DateTime.Now, 22));
		};

		It should_ignore_the_events = () => Subject.State.Number.Should().Be(42);
		It should_not_update_its_serial = () => Subject.Serial.Should().Be(10);
	}

	[Subject(typeof(EventModel<>), "initialized with a snapshot")]
	public class When_receiving_events_past_its_serial : SnapshotEventModelSetup
	{
		Because of = () =>
		{
			Events.OnNext(new CommittedEvent(11, "test", DateTime.Now, 20));
			Events.OnNext(new CommittedEvent(12, "test", DateTime.Now, 22));
		};

		It should_apply_the_events = () => Subject.State.Number.Should().Be(84);
		It should_update_its_serial_accordingly = () => Subject.Serial.Should().Be(12);
	}

}
