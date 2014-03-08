using System;
using System.Runtime.Serialization.Formatters;
using System.Text;
using EStoria.Interfaces;
using Newtonsoft.Json;

namespace EStoria.Services.Serializers
{
	public class JsonSerializer : ISerializer
	{
		readonly JsonSerializerSettings _settings;

		public JsonSerializer()
		{
			_settings = new JsonSerializerSettings
			{
				DateFormatHandling = DateFormatHandling.IsoDateFormat, 
				DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		public byte[] Serialize<T>(T @object)
		{
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@object, typeof(T), Formatting.Indented, _settings));
		}

		public T Deserialize<T>(byte[] data)
		{
			return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data), _settings);
		}
	}
}