using System;
using System.Collections.Generic;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(MessageBus))]
	public class When_requesting_publishers : WithSubject<MessageBus>
	{
		It should_return_a_valid_publisher_for_the_requested_type = () => Subject.AsPublisher<string>().Should().NotBeNull();
		It should_return_two_different_publishers_if_requested_with_different_types = () => Subject.AsPublisher<string>().Should().NotBeSameAs(Subject.AsPublisher<int>());
		It should_return_two_different_publishers_if_requested_with_the_same_type = () => Subject.AsPublisher<string>().Should().NotBeSameAs(Subject.AsPublisher<string>());
	}

	[Subject(typeof(MessageBus))]
	public class When_requesting_observables : WithSubject<MessageBus>
	{
		It should_return_a_valid_observable_for_the_requested_type = () => Subject.AsObservable<string>().Should().NotBeNull();
		It should_return_two_different_observables_if_requested_with_different_types = () => Subject.AsObservable<string>().Should().NotBeSameAs(Subject.AsObservable<int>());
		It should_return_two_different_observables_if_requested_with_the_same_type = () => Subject.AsObservable<string>().Should().NotBeSameAs(Subject.AsObservable<string>());
	}

	[Subject(typeof(MessageBus))]
	public class When_observing_and_publishing_typed_notifications : WithSubject<MessageBus>
	{
		static List<string> Strings;
		static List<int> Ints;
		static List<object> All;
		
		Establish context = () =>
		{
			Strings = new List<string>();
			Ints = new List<int>();
			All = new List<object>();
			Subject.AsObservable<string>().Subscribe(Strings.Add);
			Subject.AsObservable<int>().Subscribe(Ints.Add);
			Subject.AsObservable<object>().Subscribe(All.Add);
		};

		Because of = () =>
		{
			Subject.AsPublisher<string>().Publish("Hello");
			Subject.AsPublisher<int>().Publish(42);
			Subject.AsPublisher<object>().Publish("A string");
		};

		It should_notify_observables_based_on_the_publisher_type = () =>
		{
			Strings.Should().ContainInOrder("Hello");
			Ints.Should().ContainInOrder(42);
			All.Should().ContainInOrder("Hello", 42, "A string");
		};
	}
}