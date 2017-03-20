namespace DentalSoft.Data.Repository.Base
{
    using DentalSoft.Data.Repository.Interfaces;
    using System;
    using System.Collections;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;

    public static class UnitOfWork
    {   
        public static void Commit()
        {
            IUnitOfWork unitOfWork = GetUnitOfWork();

            if (unitOfWork != null)
            {
                unitOfWork.Commit();
            }
        }

        public static IUnitOfWork Current 
        {
            get
            {
                IUnitOfWork unitOfWork = GetUnitOfWork();

                if (unitOfWork == null)
                {

                    _unitOfWorkFactory = (IUnitOfWorkFactory)DependencyResolver.Current.GetService(typeof(IUnitOfWorkFactory));
                    unitOfWork = _unitOfWorkFactory.Create();
                    SaveUnitOfWork(unitOfWork);
                }

                return unitOfWork;
            }
        }

        #region Private Members

        private static IUnitOfWork GetUnitOfWork()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(HTTPCONTEXTKEY))
                {
                    return (IUnitOfWork)HttpContext.Current.Items[HTTPCONTEXTKEY];
                }

                return null;
            }
            else
            {
                Thread thread = Thread.CurrentThread;
                if (string.IsNullOrEmpty(thread.Name))
                {
                    thread.Name = Guid.NewGuid().ToString();
                    return null;
                }
                else
                {
                    lock (_threads.SyncRoot)
                    {
                        return (IUnitOfWork)_threads[Thread.CurrentThread.Name];
                    }
                }
            }
        }

        private static void SaveUnitOfWork(IUnitOfWork unitOfWork)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[HTTPCONTEXTKEY] = unitOfWork;
            }
            else
            {
                lock (_threads.SyncRoot)
                {
                    _threads[Thread.CurrentThread.Name] = unitOfWork;
                }
            }
        }

        private const string HTTPCONTEXTKEY = "DentalSoft.Repository.Base.HttpContext.Key";

        private static IUnitOfWorkFactory _unitOfWorkFactory;
        private static readonly Hashtable _threads = new Hashtable(); 

        #endregion
    }
}
