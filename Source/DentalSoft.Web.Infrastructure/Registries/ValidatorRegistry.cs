namespace DentalSoft.Web.Infrastructure.Registries
{
    using DentalSoft.Web.Infrastructure.Contracts.Validators;
    using Ninject;
    using Ninject.Extensions.Conventions;

    public class ValidatorBindingsRegister : INinjectRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind(k => k.FromThisAssembly()
                .SelectAllClasses()
                .InheritedFromAny(typeof(IValidator<>))
                .BindSingleInterface());
        }
    }
}
