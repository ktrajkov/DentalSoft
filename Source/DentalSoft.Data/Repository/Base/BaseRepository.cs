namespace DentalSoft.Data.Repository.Base
{
    using DentalSoft.Common.Extensions;
    using DentalSoft.Data.Repository.Interfaces;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {       
        protected IApplicationDbContext Context
        {
            get
            {
                if (context == null)
                {
                    context = GetCurrentUnitOfWork<EFUnitOfWork>().Context;
                }

                return context;
            }
        }
 
        protected IDbSet<T> DbSet
        {
            get
            {
                if (dbSet == null)
                {
                    dbSet = this.Context.Set<T>();
                }

                return dbSet;
            }
        }

        public virtual IQueryable<T> GetQuery()
        {
            return this.DbSet.AsQueryable();
        }



        public virtual IQueryable<T> All()
        {
            return this.GetQuery();
        }

        public virtual IQueryable<T> All<TFilter>(TFilter filter)
        {
            return this.GetQuery().Filter<T, TFilter>(filter);
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(object id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
                this.SaveChanges();
            }
        }

        public virtual void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public TUnitOfWork GetCurrentUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
        {
            return (TUnitOfWork)UnitOfWork.Current;
        }

        #region Private Members

        private IDbSet<T> dbSet;
        private IApplicationDbContext context;   

        #endregion
    }
}
