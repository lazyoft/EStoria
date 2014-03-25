using System;
using System.Collections.Generic;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface IHandlesCommand<in T>
	{
		IEnumerable<DomainEvent> Apply(T command);
	}
}