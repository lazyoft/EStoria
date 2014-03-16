using System;
using EStoria.Interfaces;

namespace EStoria.ValueObjects
{
	public class CommandFailure
	{
		public CommandFailure(ICommand command, string reason)
		{
			Guard.NotNull(() => command);
			Guard.NotNullOrWhiteSpace(() => reason);

			Command = command;
			Reason = reason;
		}

		public ICommand Command { get; private set; }
		public string Reason { get; private set; }
	}
}