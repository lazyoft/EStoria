using System;
using System.IO;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public class FlatFolderFileCommitStrategy : IFileCommitStrategy
	{
		public string Extension { get; private set; }

		public FlatFolderFileCommitStrategy(string extension)
		{
			Extension = extension;
		}
		
		public string FileName(CommitInfo info)
		{
			return string.Format("{0:000000}_{1}{2}", info.Serial, info.Name, Extension);
		}

		public CommitInfo CommitInfo(string path)
		{
			// ReSharper disable once PossibleNullReferenceException
			var parts = Path.GetFileNameWithoutExtension(path).Split(new[] { '_' }, 2);
			return new CommitInfo(Int32.Parse(parts[0]), parts[1]);
		}
	}
}