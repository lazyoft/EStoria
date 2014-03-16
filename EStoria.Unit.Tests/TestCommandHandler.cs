using System;
using System.Collections.Generic;
using EStoria.Interfaces;

namespace EStoria.Unit.Tests
{
	public class TestCommandHandler : CommandHandler, IHandlesCommand<TestCommand1>, IHandlesCommand<TestCommand2>, IHandlesCommand<FailingCommand>
	{
		public bool DefaultCalled;

		public TestCommandHandler(IModelLoader loader) : base(loader) {}

		public override IEnumerable<IDomainEvent> Apply(ICommand command)
		{
			DefaultCalled = true;
			return base.Apply(command);
		}

		public IEnumerable<IDomainEvent> Apply(TestCommand1 command)
		{
			yield return new TestDomainEvent { Value = "Event-" + command.Value };
		}

		public IEnumerable<IDomainEvent> Apply(TestCommand2 command)
		{
			yield return new TestDomainEvent { Value = "Event1-" + command.Value };
			yield return new TestDomainEvent { Value = "Event2-" + command.Value };				
		}

		public IEnumerable<IDomainEvent> Apply(FailingCommand command)
		{
			return FailCommand(command, "Some reason");
		}
	}

	public class TestDomainEvent : IDomainEvent { public string Value { get; set; } public string AggregateId { get; set; } }
	public class TestCommand1 : ICommand { public string Value { get; set; } }
	public class UnknownCommand : ICommand { public string Value { get; set; } }
	public class FailingCommand : ICommand { }
	public class TestCommand2 : TestCommand1 { }
}