using System;
using EStoria.Services.Repositories.FileSystem;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(FlatFolderFileEventStrategy))]
	public class When_asked_to_create_file_names_and_EventInfos : WithSubject<FlatFolderFileEventStrategy>
	{
		It should_return_the_file_name_with_both_the_stream_and_the_serial = () => Subject.FileName(new EventInfo(42, "test")).Should().Be("000042_test.event");
		It should_return_an_EventInfo_parsed_from_the_file_name = () => Subject.EventInfo("000042_test.event").Should().Be(new EventInfo(42, "test"));
		It should_allow_underscores_in_the_stream_name = () => Subject.EventInfo("000042_test_with_underscores.event").Should().Be(new EventInfo(42, "test_with_underscores"));
		It should_revert_to_and_from_an_EventInfo = () => Subject.EventInfo(Subject.FileName(new EventInfo(42, "test"))).Should().Be(new EventInfo(42, "test"));
		It should_revert_to_and_from_a_file_name = () => Subject.FileName(Subject.EventInfo("000042_test.event")).Should().Be("000042_test.event");
	}
}
