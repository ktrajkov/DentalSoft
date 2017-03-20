
namespace DentalSoft.Data.Services.Interfaces
{
    public interface IEntityDeleting<TEntity>
    {
        void Deleting(TEntity entity);
    }
}
