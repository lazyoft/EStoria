using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using EStoria.Interfaces;
using EStoria.ValueObjects;
using FluentAssertions;
using Machine.Specifications;

namespace EStoria.Sample.Unit.Tests
{
	public class CommandHandlerSpec
	{
		readonly IList<CommandFailure> _failures;
		readonly IList<DomainEvent> _events;
		readonly ICommandHandler _handler;
		
		int _serial;
		bool _givenCalled;
		ICommand _issuedCommand;

		public ISubject<CommittedEvent> CommittedEvents { get; private set; }

		public CommandHandlerSpec(ICommandHandler handler)
		{
			CommittedEvents = new Subject<CommittedEvent>();
			_handler = handler;
			_events = new List<DomainEvent>();
			_failures = new List<CommandFailure>();
			_handler.Subscribe<DomainEvent>(evt => _events.Add(evt), e => { }, () => { });
			_handler.Subscribe<CommandFailure>(failure => _failures.Add(failure), e => { }, () => { });
		}

		public CommandHandlerSpec Given(params object[] events)
		{
			_givenCalled = true;
			Debug.WriteLine("Given:");
			foreach(var @event in events)
			{
				Debug.WriteLine("\t- {0}", @event);
				CommittedEvents.OnNext(new CommittedEvent(++_serial, "", DateTime.Now, @event));
			}
			return this;
		}

		public CommandHandlerSpec WhenIssuing(ICommand command)
		{
			if(!_givenCalled)
				Debug.WriteLine("Given no previous events");

			Debug.WriteLine("When issuing:");
			Debug.WriteLine("\t- {0}", command);
			_issuedCommand = command;
			_handler.OnNext(command);
			return this;
		}

		public void ShouldRaise(params object[] events)
		{
			Debug.WriteLine("Should raise:");
			foreach(var @event in events)
				Debug.WriteLine("\t- {0}", @event);
			try
			{
				_events.Select(e => e.Event.GetType().Name).Should().ContainInOrder(events.Select(e => e.GetType().Name));
				_events.Select(e => e.Event).ShouldAllBeEquivalentTo(events);
			}
			catch(SpecificationException)
			{
				Debug.WriteLine("Received the following events:");
				foreach(var @event in events)
					Debug.WriteLine("\t- {0}", @event);
				Debug.WriteLine("Expecting instead:");
				foreach(var @event in _events.Select(e => e.Event))
					Debug.WriteLine("\t- {0}", @event);
				throw;
			}
		}

		public void ShouldNotRaiseAnything()
		{
			Debug.WriteLine("Should not raise any event.");
			try
			{
				_events.Should().BeEmpty();
			}
			catch(SpecificationException)
			{
				Debug.WriteLine("Received the following events:");
				foreach (var @event in _events.Select(e => e.Event))
					Debug.WriteLine("\t- {0}", @event);
				Debug.WriteLine("Expecting instead no events.");
				throw;
			}
		}

		public void ShouldFailTheCommand()
		{
			ShouldNotRaiseAnything();
			Debug.WriteLine("Should fail the command.");
			_failures.Select(f => f.Command).Should().ContainInOrder(_issuedCommand);
		}
	}
}