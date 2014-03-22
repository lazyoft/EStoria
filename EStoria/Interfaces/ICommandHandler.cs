using System;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface ICommandHandler : IObserver<ICommand>, IObservable<DomainEvent>, IObservable<CommandFailure>, IHandlesCommand<ICommand> { }
}