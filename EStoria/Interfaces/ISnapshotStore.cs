using System;
using EStoria.ValueObjects;

namespace EStoria.Interfaces
{
	public interface ISnapshotStore
	{
		CommittedSnapshot Append(string snapshotName, int serial, object memento);
		CommittedSnapshot Read(string snapshotName, int closeToSerial = Int32.MaxValue);
	}
}