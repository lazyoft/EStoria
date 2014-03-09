using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public class FileCommitPersistence : ICommitPersistence
	{
		readonly string _basePath;
		readonly IFileCommitStrategy _fileCommitStrategy;

		public FileCommitPersistence(string basePath, IFileCommitStrategy fileCommitStrategy)
		{
			_basePath = basePath;
			_fileCommitStrategy = fileCommitStrategy;
		}

		public void Save(CommitInfo info, byte[] data)
		{
			File.WriteAllBytes(Path.Combine(_basePath, _fileCommitStrategy.FileName(info)), data);
		}

		public IQueryable<CommitInfo> GetInfos()
		{
			return (from file in Directory.EnumerateFiles(_basePath, "*" + _fileCommitStrategy.Extension, SearchOption.AllDirectories)
				orderby file
				select _fileCommitStrategy.CommitInfo(file)).AsQueryable();
		}

		public int GetLastSerial()
		{
			return GetInfos().Select(i => i.Serial).LastOrDefault();
		}

		public byte[] Read(CommitInfo info)
		{
			return File.ReadAllBytes(Path.Combine(_basePath, _fileCommitStrategy.FileName(info)));
		}
	}
}