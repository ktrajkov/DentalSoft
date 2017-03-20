namespace DentalSoft.Data.Services.Interfaces
{
    /// <summary>
    /// The interface for domain entity deleted event.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public interface IEntityDeleted<T>
    {
        /// <summary>
        /// Handles the entity deleted event.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Deleted(T entity);
    }
}
