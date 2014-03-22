using System;
using System.Collections.Generic;

namespace EStoria.Interfaces
{
	public interface IHandlesCommand<in T>
	{
		IEnumerable<DomainEvent> Apply(T command);
	}
}