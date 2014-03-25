using System;
using System.Diagnostics;
using EStoria.Interfaces;
using Machine.Fakes;
using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Sdk;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace EStoria.Sample.Unit.Tests
{
	[SetupForEachSpecification]
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public class WithHandler<T, T1>
		where T : class, ICommandHandler
		where T1 : IEventModel
	{
		protected static CommandHandlerSpec Handler { get; private set; }

		Establish context = () =>
		{
			var specificationController = new SpecificationController<T, NSubstituteEngine>();
			specificationController.EnsureSubjectCreated();

			Debug.WriteLine("** {0}, handling {1} **", typeof(T).Name, typeof(T1).Name);
			Handler = new CommandHandlerSpec(specificationController.Subject);
			var aggregate = (T1)typeof(T1).GetConstructors()[0].Invoke(new object[] { Handler.CommittedEvents, null, 0 });
			specificationController.The<IModelLoader>()
				.WhenToldTo(loader => loader.Load<T1>(Param.IsAny<string>()))
				.Return(aggregate);
		};
	}
}