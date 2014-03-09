using System;
using System.Collections.Generic;
using System.Linq;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(SnapshotStore))]
	public class When_saving_a_snapshot : WithSubject<SnapshotStore>
	{
		static CommittedSnapshot SavedSnapshot;

		Establish context = () => The<IClock>().WhenToldTo(c => c.Now()).Return(new DateTime(1971, 11, 26, 12, 30, 00));

		Because of = () => SavedSnapshot = Subject.Append("snapshot", 42, "some memento");

		It should_return_a_CommittedSnapshot = () => SavedSnapshot.Should().NotBeNull();
		It should_assign_the_CommittedSnapshot_a_timeStamp = () => SavedSnapshot.TimeStamp.Should().Be(new DateTime(1971, 11, 26, 12, 30, 00));
		It should_assign_the_CommittedSnapshot_the_passed_serial = () => SavedSnapshot.Serial.Should().Be(42);
		It should_assign_the_CommittedSnapshot_the_given_name = () => SavedSnapshot.Name.Should().Be("snapshot");
		It should_assign_the_CommittedSnapshot_the_given_memento = () => SavedSnapshot.Data.Should().Be("some memento");
		It should_request_to_save_the_given_snapshot = () => The<ISnapshotPersistence>().WasToldTo(er => er.Save(new CommitInfo(42, "snapshot"), Arg.Any<byte[]>())).OnlyOnce();
		It should_request_to_serialize_the_given_snapshot = () => The<ISerializer>().WasToldTo(s => s.Serialize(SavedSnapshot)).OnlyOnce();
	}

	[Subject(typeof(SnapshotStore))]
	public class When_asked_to_save_a_snapshot_with_invalid_arguments : WithSubject<SnapshotStore>
	{
		It should_throw_an_ArgumentNullException_if_being_passed_a_null_memento = () => Subject.Invoking(s => s.Append("snapshot", 42, default(object))).ShouldThrow<ArgumentNullException>();
		It should_throw_an_ArgumentException_if_being_passed_a_null_stream_name = () => Subject.Invoking(s => s.Append(null, 42, "some event data")).ShouldThrow<ArgumentException>();
		It should_throw_an_ArgumentException_if_being_passed_an_empty_stream_name = () => Subject.Invoking(s => s.Append(string.Empty, 42, "some event data")).ShouldThrow<ArgumentException>();
		It should_throw_an_ArgumentException_if_being_passed_a_stream_name_made_of_only_whitespace = () => Subject.Invoking(s => s.Append("   ", 42, "some event data")).ShouldThrow<ArgumentException>();
	}

	public class SnapshotStoreSetup : WithSubject<SnapshotStore>
	{
		protected static IList<CommitInfo> Infos;

		Establish context = () =>
		{
			Infos = new List<CommitInfo>();
			The<ISnapshotPersistence>().WhenToldTo(er => er.GetInfos()).Return(new[] { new CommitInfo(1, "snapshot1"), new CommitInfo(42, "snapshot1"), new CommitInfo(78, "snapshot2") }.AsQueryable());
			The<ISnapshotPersistence>().When(er => er.Read(Arg.Any<CommitInfo>())).Do(ci => Infos.Add(ci.Arg<CommitInfo>()));
		};
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_the_most_upddated_snapshot : SnapshotStoreSetup
	{
		Because of = () => Subject.Read("snapshot1");
		It should_return_the_most_updated_snapshot = () => Infos.Should().Contain(new CommitInfo(42, "snapshot1"));
	}

	[Subject(typeof(EventStore))]
	public class When_asked_to_read_a_specific_serial_of_snapshot : SnapshotStoreSetup
	{
		Because of = () => Subject.Read("snapshot1", 10);
		It should_return_the_most_updated_snapshot_before_or_equal_to_the_passed_serial = () => Infos.Should().Contain(new CommitInfo(1, "snapshot1"));
	}
}
