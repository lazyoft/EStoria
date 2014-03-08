using System;
using EStoria.Services.SerialProviders;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(ThreadSafeSerialProvider))]
	public class When_asked_for_the_next_serial : WithSubject<ThreadSafeSerialProvider>
	{
		It should_return_1_on_the_first_call = () => Subject.Next().Should().Be(1);
		It should_increment_the_serial_on_subsequent_calls = () => Subject.Next().Should().Be(2);
	}
}
