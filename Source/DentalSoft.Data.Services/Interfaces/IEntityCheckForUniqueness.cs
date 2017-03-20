namespace DentalSoft.Data.Services.Interfaces
{
    public interface IEntityCheckForUniqueness<TEntity>
    {
        void CheckForUniqueness(TEntity entity);
    }
}
