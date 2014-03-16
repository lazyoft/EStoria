using System;

namespace EStoria.Interfaces
{
	public interface IModelLoader
	{
		T Load<T>(string id) where T : IEventModel;
	}
}