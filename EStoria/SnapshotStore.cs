using System;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;

namespace EStoria
{
	public class SnapshotStore : ISnapshotStore
	{
		readonly ISnapshotPersistence _persistence;
		readonly IClock _clock;
		readonly ISerializer _serializer;

		public SnapshotStore(ISnapshotPersistence persistence, IClock clock, ISerializer serializer)
		{
			Guard.NotNull(() => persistence);
			Guard.NotNull(() => clock);
			Guard.NotNull(() => serializer);

			_persistence = persistence;
			_clock = clock;
			_serializer = serializer;
		}

		public CommittedSnapshot Append(string snapshotName, int serial, object memento)
		{
			Guard.NotNullOrWhiteSpace(() => snapshotName);
			Guard.NotNull(() => memento);

			var result = new CommittedSnapshot(serial, snapshotName, _clock.Now(), memento);
			_persistence.Save(new CommitInfo(serial, snapshotName), _serializer.Serialize(result));
			return result;
		}

		public CommittedSnapshot Read(string snapshotName, int closeToSerial = Int32.MaxValue)
		{
			Guard.NotNullOrWhiteSpace(() => snapshotName);

			var snapshotInfo = (from info in _persistence.GetInfos()
				where (info.Name == snapshotName) && info.Serial <= closeToSerial
				orderby info.Serial descending
				select info).FirstOrDefault();

			return snapshotInfo == null ? new CommittedSnapshot(0, snapshotName, _clock.Now(), null) : _serializer.Deserialize<CommittedSnapshot>(_persistence.Read(snapshotInfo));
		}
	}
}