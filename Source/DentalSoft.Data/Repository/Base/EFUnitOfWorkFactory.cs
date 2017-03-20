namespace DentalSoft.Data.Repository.Base
{
    using DentalSoft.Data.Repository.Interfaces;
    using System;

    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {   
        public static void SetDbContext(Func<IApplicationDbContext> dbContextDelegate)
        {
            _dbContextDelegate = dbContextDelegate;
        }

        public IUnitOfWork Create()
        {
            IApplicationDbContext context;

            lock (_lockObject)
            {
                context = _dbContextDelegate();
            }

            return new EFUnitOfWork(context);
        }

        #region Private Members

        private static Func<IApplicationDbContext> _dbContextDelegate;
        private static readonly Object _lockObject = new object(); 

        #endregion
    }
}
