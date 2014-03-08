using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public class FileEventRepository : IEventRepository
	{
		readonly string _basePath;
		readonly IFileEventStrategy _fileEventStrategy;

		public FileEventRepository(string basePath, IFileEventStrategy fileEventStrategy)
		{
			_basePath = basePath;
			_fileEventStrategy = fileEventStrategy;
		}

		public void Save(EventInfo info, byte[] data)
		{
			File.WriteAllBytes(Path.Combine(_basePath, _fileEventStrategy.FileName(info)), data);
		}

		public IEnumerable<EventInfo> GetInfos()
		{
			return from file in Directory.EnumerateFiles(_basePath, "*.event", SearchOption.AllDirectories)
				orderby file
				select _fileEventStrategy.EventInfo(file);
		}

		public int GetLastSerial()
		{
			return GetInfos().Select(i => i.Serial).LastOrDefault();
		}

		public byte[] Read(EventInfo info)
		{
			return File.ReadAllBytes(Path.Combine(_basePath, _fileEventStrategy.FileName(info)));
		}
	}
}