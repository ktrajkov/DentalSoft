namespace DentalSoft.Data.Repository.Base
{
    using DentalSoft.Data.Repository.Interfaces;
    using System;

    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        public IApplicationDbContext Context { get; private set; }

        public EFUnitOfWork(IApplicationDbContext context)
        {          
            Context = context;
            Context.DbContext.Configuration.LazyLoadingEnabled = true;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
