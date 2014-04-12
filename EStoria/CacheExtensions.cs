using System;
using EStoria.Interfaces;

namespace EStoria
{
	public static class CacheExtensions
	{
		public static T GetOrStore<T>(this ICache cache, string key, Func<T> factory)
		{
			if(!cache.Contains(key))
				cache[key] = factory();
			return (T)cache[key];
		}
	}
}