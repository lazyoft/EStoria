using System;
using System.Runtime.Caching;
using EStoria.Interfaces;

namespace EStoria.Services
{
	public class InMemoryCache : ICache
	{
		public bool Contains(string key)
		{
			return MemoryCache.Default.Contains(key);
		}

		public object this[string key]
		{
			get
			{
				return MemoryCache.Default[key];
			}
			set
			{
				MemoryCache.Default[key] = value;
			}
		}
	}
}