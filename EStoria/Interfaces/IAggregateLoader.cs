using System;

namespace EStoria.Interfaces
{
	public interface IAggregateLoader
	{
		T Load<T, TModel>(string id) where T : EventModel<TModel> where TModel : class, new();
	}
}