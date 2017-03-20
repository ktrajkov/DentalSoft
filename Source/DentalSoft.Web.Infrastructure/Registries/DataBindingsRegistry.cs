namespace DentalSoft.Web.Infrastructure.Registries
{
    using DentalSoft.Data;
    using DentalSoft.Data.Repository.Base;
    using DentalSoft.Data.Repository.Interfaces;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using Ninject;
    using Ninject.Web.Common;

    public class DataBindingsRegister : INinjectRegistry
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<IUnitOfWorkFactory>().To<EFUnitOfWorkFactory>().InRequestScope();
            kernel.Bind<IApplicationDbContext>().To<ApplicationDbContext>().InRequestScope();
            kernel.Bind(typeof(IBaseRepository<>)).To(typeof(BaseRepository<>));
            kernel.Bind(typeof(IDeletableRepository<>)).To(typeof(DeletableRepository<>));
            kernel.Bind(typeof(IRepository<,>)).To(typeof(Repository<,>));
        }
    }
}
