namespace DentalSoft.Web.Infrastructure.Registries
{
    using DentalSoft.Common.Constants;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using DentalSoft.Services.Contracts;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;

    public class ServiceBindingsRegister : INinjectRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind(k => k.FromAssemblyContaining<IService>()
                .SelectAllClasses()
                .BindAllInterfaces()
                .Configure(b => b.InRequestScope()));

            kernel.Bind(k => k.From(Assemblies.Data)
                .SelectAllClasses()
                .InheritedFromAny(typeof(IEntityCheckForUniqueness<>),
                                    typeof(IContractFilled<,>),
                                    typeof(IEntityFilled<,>),
                                    typeof(IEntityDeleting<>),
                                    typeof(IEntitySaved<>),
                                    typeof(IEntityFilling<,>))
                .BindSingleInterface());
        }
    }
}
