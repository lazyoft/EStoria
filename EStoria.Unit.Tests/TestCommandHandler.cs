using System;
using System.Collections.Generic;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria.Unit.Tests
{
	public class TestCommandHandler : CommandHandler, IHandlesCommand<TestCommand1>, IHandlesCommand<TestCommand2>, IHandlesCommand<FailingCommand>
	{
		public bool DefaultCalled;

		public override IEnumerable<DomainEvent> Apply(ICommand command)
		{
			DefaultCalled = true;
			return base.Apply(command);
		}

		public IEnumerable<DomainEvent> Apply(TestCommand1 command)
		{
			yield return new DomainEvent { Event = "Event-" + command.Value };
		}

		public IEnumerable<DomainEvent> Apply(TestCommand2 command)
		{
			yield return new DomainEvent { Event = "Event1-" + command.Value };
			yield return new DomainEvent { Event = "Event2-" + command.Value };				
		}

		public IEnumerable<DomainEvent> Apply(FailingCommand command)
		{
			FailCommand(command, "Some reason");
			yield break;
		}
	}

	public class TestCommand1 : ICommand { public string Value { get; set; } }
	public class UnknownCommand : ICommand { public string Value { get; set; } }
	public class FailingCommand : ICommand { }
	public class TestCommand2 : TestCommand1 { }
}