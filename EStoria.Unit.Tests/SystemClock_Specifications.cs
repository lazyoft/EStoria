using System;
using EStoria.Services;
using FluentAssertions;
using Machine.Fakes;
using Machine.Specifications;

namespace EStoria.Unit.Tests
{
	[Subject(typeof(SystemClock))]
	public class When_asked_for_the_time : WithSubject<SystemClock>
	{
		It should_return_the_current_time = () => Subject.Now().Should().BeCloseTo(DateTime.Now);
	}
}
