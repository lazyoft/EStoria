using System;

namespace EStoria.Interfaces
{
	public interface IModelLoader
	{
		T Load<T, TModel>(string id) where T : EventModel<TModel> where TModel : class, new();
	}
}