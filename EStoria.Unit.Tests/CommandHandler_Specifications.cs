using System;
using System.Collections.Generic;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	public class CommandHandlerSetup : WithSubject<TestCommandHandler>
	{
		protected static IList<IDomainEvent> Events;
		protected static IEnumerable<string> EventValues;
		protected static CommandFailure Failure;
		protected static Exception Error;
		protected static bool Completed;

		Establish context = () =>
		{
			Events = new List<IDomainEvent>();
			EventValues = Events.Cast<TestDomainEvent>().Select(evt => evt.Value);
			Subject.Subscribe<IDomainEvent>(evt => Events.Add(evt), e => Error = e, () => Completed = true);
			Subject.Subscribe<CommandFailure>(failure => Failure = failure);
		};

	}

	[Subject(typeof(CommandHandler))]
	public class When_receiving_commands : CommandHandlerSetup
	{
		Because of = () =>
		{
			Subject.OnNext(new TestCommand1 { Value = "test1" });
			Subject.OnNext(new TestCommand2 { Value = "test2" });
		};
		It should_publish_events_according_to_the_command = () => EventValues.Should().ContainInOrder("Event-test1", "Event1-test2", "Event2-test2");
	}

	[Subject(typeof(CommandHandler))]
	public class When_receiving_unknown_commands : CommandHandlerSetup
	{
		Because of = () => Subject.OnNext(new UnknownCommand { Value = "unknown" });

		It should_not_publish_any_event = () => EventValues.Should().BeEmpty();
		It should_process_the_command_with_the_default_handler = () => Subject.DefaultCalled.Should().BeTrue();
	}

	[Subject(typeof(CommandHandler))]
	public class When_receiving_an_error : CommandHandlerSetup
	{
		Because of = () => Subject.OnError(new Exception("Some exception"));

		It should_not_publish_any_event = () => EventValues.Should().BeEmpty();
		It should_publish_the_error = () => Error.Should().NotBeNull();
	}

	[Subject(typeof(CommandHandler))]
	public class When_receiving_a_completion : CommandHandlerSetup
	{
		Because of = () => Subject.OnCompleted();

		It should_not_publish_any_event = () => EventValues.Should().BeEmpty();
		It should_complete = () => Completed.Should().BeTrue();
	}

	[Subject(typeof(CommandHandler))]
	public class When_failing_a_command : CommandHandlerSetup
	{
		Because of = () => Subject.OnNext(new FailingCommand());

		It should_not_publish_any_event = () => EventValues.Should().BeEmpty();
		It should_publish_the_failure = () => Failure.Should().NotBeNull();
	}
}