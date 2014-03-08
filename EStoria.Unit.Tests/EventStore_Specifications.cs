using System;
using System.Collections.Generic;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(EventStore))]
	public class When_saving_a_single_event : WithSubject<EventStore>
	{
		static CommittedEvent SavedEvent;

		Establish context = () =>
		{
			The<IClock>().WhenToldTo(c => c.Now()).Return(new DateTime(1971, 11, 26, 12, 30, 00));
			The<ISerialProvider>().WhenToldTo(sp => sp.Next()).Return(1);
		};

		Because of = () => SavedEvent = Subject.Append("stream", "some event data");

		It should_return_a_CommittedEvent = () => SavedEvent.Should().NotBeNull();
		It should_assign_the_CommittedEvent_a_timeStamp = () => SavedEvent.TimeStamp.Should().Be(new DateTime(1971, 11, 26, 12, 30, 00));
		It should_assign_the_CommittedEvent_a_serial = () => SavedEvent.Serial.Should().Be(1);
		It should_assign_the_CommittedEvent_the_given_stream = () => SavedEvent.StreamName.Should().Be("stream");
		It should_assign_the_CommittedEvent_the_given_event_data = () => SavedEvent.Event.Should().Be("some event data");
		It should_request_to_save_the_given_event = () => The<IEventRepository>().WasToldTo(er => er.Save(new EventInfo(1, "stream"), Arg.Any<byte[]>())).OnlyOnce();
		It should_request_to_serialize_the_given_event = () => The<ISerializer>().WasToldTo(s => s.Serialize(SavedEvent)).OnlyOnce();
	}

	[Subject(typeof(EventStore))]
	public class When_saving_multiple_events : WithSubject<EventStore>
	{
		static List<CommittedEvent> SavedEvents;
		static int Counter;

		Establish context = () =>
		{
			The<IClock>().WhenToldTo(c => c.Now()).Return(new DateTime(1971, 11, 26, 12, 30, 00));
			The<ISerialProvider>().WhenToldTo(sp => sp.Next()).Return(() => ++Counter);
		};

		Because of = () => SavedEvents = new List<CommittedEvent>(Subject.Append("stream", new[] { "event 1", "event 2", "event 3" }));

		It should_return_a_sequence_of_CommittedEvents = () => SavedEvents.Should().NotBeNull().And.HaveCount(3);
		It should_assign_the_CommittedEvents_a_TimeStamp = () => SavedEvents.Should().Contain(evt => evt.TimeStamp == new DateTime(1971, 11, 26, 12, 30, 00));
		It should_assign_the_CommittedEvents_a_serial = () => SavedEvents.Select(e => e.Serial).Should().ContainInOrder(1, 2, 3);
		It should_assign_the_CommittedEvents_the_given_stream = () => SavedEvents.Should().Contain(evt => evt.StreamName == "stream");
		It should_assign_the_CommittedEvents_the_given_event_data = () => SavedEvents.Select(e => e.Event.ToString()).Should().ContainInOrder("event 1", "event 2", "event 3");
		It should_request_to_save_the_given_events = () => The<IEventRepository>().WasToldTo(er => er.Save(Arg.Any<EventInfo>(), Arg.Any<byte[]>())).Times(3);
		It should_request_to_serialize_the_given_events = () => The<ISerializer>().WasToldTo(s => s.Serialize(Arg.Any<CommittedEvent>())).Times(3);
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_save_with_invalid_arguments : WithSubject<EventStore>
	{
		It should_throw_an_ArgumentNullException_if_being_passed_a_null_object = () => Subject.Invoking(s => s.Append("stream", default(object))).ShouldThrow<ArgumentNullException>();
		It should_throw_an_ArgumentNullException_if_being_passed_a_sequence_containing_a_null_object = () => Subject.Invoking(s => s.Append("stream", new[] { "event 1", null, "event 2"})).ShouldThrow<ArgumentNullException>();
		It should_throw_an_ArgumentException_if_being_passed_a_null_stream_name = () => Subject.Invoking(s => s.Append(null, "some event data")).ShouldThrow<ArgumentException>();
		It should_throw_an_ArgumentException_if_being_passed_an_empty_stream_name = () => Subject.Invoking(s => s.Append(string.Empty, "some event data")).ShouldThrow<ArgumentException>();
		It should_throw_an_ArgumentException_if_being_passed_a_stream_name_made_of_only_whitespace = () => Subject.Invoking(s => s.Append(string.Empty, "some event data")).ShouldThrow<ArgumentException>();
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_save_a_sequence_of_events_containing_a_null : WithSubject<EventStore>
	{
		Because of = () => Catch.Exception(() => Subject.Append("stream", new[] { "event 1", null, "event 2" }));
		It should_not_persist_any_event = () => The<IEventRepository>().WasNotToldTo(er => er.Save(Arg.Any<EventInfo>(), Arg.Any<byte[]>()));
	}

	public class DurableStoreReadSetup : WithSubject<EventStore>
	{
		protected static IList<EventInfo> ReadEvents;

		Establish context = () =>
		{
			ReadEvents = new List<EventInfo>();
			The<IEventRepository>().WhenToldTo(er => er.GetInfos()).Return(new[] { new EventInfo(1, "stream1"), new EventInfo(2, "stream2"), new EventInfo(3, "stream1") });
			The<IEventRepository>().When(er => er.Read(Arg.Any<EventInfo>())).Do(ci => ReadEvents.Add(ci.Arg<EventInfo>()));
		};

	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_all_the_events_from_all_the_streams : DurableStoreReadSetup
	{
		Because of = () => Subject.Read().ToList();
		It should_return_all_the_events = () => ReadEvents.Should().ContainInOrder(new EventInfo(1, "stream1"), new EventInfo(2, "stream2"), new EventInfo(3, "stream1"));
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_all_the_events_starting_from_a_serial : DurableStoreReadSetup
	{
		Because of = () => Subject.Read(null, 2).ToList();
		It should_return_only_the_events_from_that_serial_onwards = () => ReadEvents.Should().ContainInOrder(new EventInfo(2, "stream2"), new EventInfo(3, "stream1"));
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_the_events_from_a_stream : DurableStoreReadSetup
	{
		Because of = () => Subject.Read("stream2").ToList();
		It should_return_only_the_events_from_that_stream = () => ReadEvents.Should().ContainInOrder(new EventInfo(2, "stream2"));
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_the_events_from_a_stream_and_from_a_serial : DurableStoreReadSetup
	{
		Because of = () => Subject.Read("stream1", 2).ToList();
		It should_return_only_the_events_from_that_stream_starting_from_that_serial_onwards = () => ReadEvents.Should().ContainInOrder(new EventInfo(3, "stream1"));
	}
}
