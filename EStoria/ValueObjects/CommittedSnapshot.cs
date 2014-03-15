using System;

namespace EStoria.ValueObjects
{
	public class CommittedSnapshot : Commit 
	{
		public CommittedSnapshot(int serial, string name, DateTime timeStamp, object data) : base(serial, name, timeStamp, data) {}
	}
}