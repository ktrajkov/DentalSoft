namespace DentalSoft.Data.Repository.Interfaces
{
    using System;
    using System.Linq;

    public interface IBaseRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        IQueryable<T> All<TFilter>(TFilter filter);

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        void Detach(T entity);

        int SaveChanges();
    }
}
