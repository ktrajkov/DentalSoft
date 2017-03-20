namespace DentalSoft.Data.Services.Interfaces
{
    using DentalSoft.Data.Repository.Interfaces;
    using System.Linq;

    public interface IRepository<TContract,TEntity> : IDeletableRepository<TEntity> where TEntity : class
    {
        TContract GetByIdToModel(object id);

        IQueryable<TContract> AllToModel();

        IQueryable<TContract> AllToModel<TFilter>(TFilter filter);

        IQueryable<TContract> AllWithDeletedToModel();

        TContract Load();

        void Save(TContract contract);

        void Delete(TContract contract);

    }
}
