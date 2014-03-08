using System;
using System.IO;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public class FlatFolderFileEventStrategy : IFileEventStrategy
	{
		public string FileName(EventInfo info)
		{
			return string.Format("{0:000000}_{1}.event", info.Serial, info.StreamName);
		}

		public EventInfo EventInfo(string path)
		{
			// ReSharper disable once PossibleNullReferenceException
			var parts = Path.GetFileNameWithoutExtension(path).Split(new[] { '_' }, 2);
			return new EventInfo(Int32.Parse(parts[0]), parts[1]);
		}
	}
}