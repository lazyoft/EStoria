using System;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface ICommandHandler : IObserver<ICommand>, IObservable<IDomainEvent>, IObservable<CommandFailure>, IHandlesCommand<ICommand> { }
}