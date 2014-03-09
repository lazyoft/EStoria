using System;
using System.Linq;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface ICommitPersistence
	{
		void Save(CommitInfo info, byte[] data);
		IQueryable<CommitInfo> GetInfos();
		byte[] Read(CommitInfo info);
	}

	public interface IEventPersistence : ICommitPersistence {}
	public interface ISnapshotPersistence : ICommitPersistence {}
}