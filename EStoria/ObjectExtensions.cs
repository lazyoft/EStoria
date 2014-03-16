using System;
using Newtonsoft.Json;

namespace EStoria
{
	public static class ObjectExtensions
	{
		public static T DeepCopy<T>(this T item)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(item));
		}
	}
}