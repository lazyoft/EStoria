using System;
using EStoria.ValueObjects;

namespace EStoria.Services.Persistence.FileSystem
{
	public interface IFileCommitStrategy
	{
		string Extension { get; }
		string FileName(CommitInfo info);
		CommitInfo CommitInfo(string fileName);
	}
}