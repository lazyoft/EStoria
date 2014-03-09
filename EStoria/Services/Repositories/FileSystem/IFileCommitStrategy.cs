using System;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public interface IFileCommitStrategy
	{
		string Extension { get; }
		string FileName(CommitInfo info);
		CommitInfo CommitInfo(string fileName);
	}
}