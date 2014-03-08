using System;

namespace EStoria.Interfaces
{
	public interface ISerializer
	{
		byte[] Serialize<T>(T @object);
		T Deserialize<T>(byte[] data);
	}
}