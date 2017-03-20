
namespace DentalSoft.Data.Repository.Interfaces
{
    public interface IDeletableRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        void ActualDelete(TEntity entity);
    }
}
