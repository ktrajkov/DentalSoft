namespace DentalSoft.Data.Services
{
    using DentalSoft.Data.Repository.Base;
    using DentalSoft.Data.Repository.Interfaces;
    using DentalSoft.Data.Services.Interfaces;
    using System.Web.Mvc;

    public static class RepositoryManager
    {
        public static void Initialize()
        {
            EFUnitOfWorkFactory.SetDbContext(() => new ApplicationDbContext());
        }

        public static void Commit()
        {
            UnitOfWork.Commit();
        }

        public static void Dispose()
        {
            UnitOfWork.Current.Dispose();
        }

        public static IRepository<TContract,TEntity> GetRepository<TContract,TEntity>() where TEntity : class
        {
            return (IRepository<TContract, TEntity>)DependencyResolver.Current.GetService(typeof(IRepository<TContract, TEntity>));
        }

        public static IBaseRepository<TEntity> GetRepositoryForEntity<TEntity>() where TEntity : class
        {
            return (IDeletableRepository<TEntity>)DependencyResolver.Current.GetService(typeof(IDeletableRepository<TEntity>) );
        }
    }
}
