using System;

namespace EStoria.Interfaces
{
	public interface ICache
	{
		bool Contains(string key);
		object this[string key] { get; set; }
	}
}