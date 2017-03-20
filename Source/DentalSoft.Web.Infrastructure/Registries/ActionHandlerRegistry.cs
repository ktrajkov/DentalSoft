namespace DentalSoft.Web.Infrastructure.Registries
{
    using ActionResults.Contracts;


    using Ninject;
    using Ninject.Extensions.Conventions;


    public class ActionHandlerBindingsRegister : INinjectRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind(k => k.FromThisAssembly()
                .SelectAllClasses()
                .InheritedFromAny(typeof(IActionHandler<>),
                                        typeof(IActionHandlerWithModel<,>),
                                        typeof(IActionHandlerWithQueryModel<,>),
                                        typeof(IActionHandlerById<,>),
                                        typeof(IPostedDataActionHandler<,>))
                .BindSingleInterface());
        }
    }
}
