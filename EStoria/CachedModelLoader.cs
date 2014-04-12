using System;
using EStoria.Interfaces;

namespace EStoria
{
	public class CachedModelLoader : IModelLoader
	{
		readonly IModelLoader _baseLoader;
		readonly ICache _cache;

		public CachedModelLoader(IModelLoader baseLoader, ICache cache)
		{
			_baseLoader = baseLoader;
			_cache = cache;
		}

		public T Load<T>(string id) where T : IEventModel
		{
			return _cache.GetOrStore(id, () => _baseLoader.Load<T>(id));
		}
	}
}