namespace DentalSoft.Data.Repository.Base
{
    using DentalSoft.Data.Models;
    using DentalSoft.Data.Repository.Interfaces;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DeletableRepository<TEntity> : BaseRepository<TEntity>, IDeletableRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {

        //Used Global filter for deleted entities
        //public override IQueryable<TEntity> GetQuery()
        //{
        //    return base.GetQuery().Where(x => !x.IsDeleted);
        //}

       
        public IQueryable<TEntity> AllWithDeleted()
        {
            return base.DbSet.AsQueryable();
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;

            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public void ActualDelete(TEntity entity)
        {
            base.Delete(entity);
        }
    }
}
