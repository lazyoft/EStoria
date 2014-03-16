using System;
using System.Collections.Generic;

namespace EStoria.Interfaces
{
	public interface IHandlesCommand<in T>
	{
		IEnumerable<IDomainEvent> Apply(T command);
	}
}