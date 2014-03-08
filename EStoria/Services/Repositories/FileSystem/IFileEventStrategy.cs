using System;
using EStoria.ValueObjects;

namespace EStoria.Services.Repositories.FileSystem
{
	public interface IFileEventStrategy
	{
		string FileName(EventInfo info);
		EventInfo EventInfo(string fileName);
	}
}