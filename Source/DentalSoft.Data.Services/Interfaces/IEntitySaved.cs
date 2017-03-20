namespace DentalSoft.Data.Services.Interfaces
{
    /// <summary>
    /// The interface for domain entity saved event.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public interface IEntitySaved<T>
    {
        /// <summary>
        /// Handles the entity saved event.
        /// </summary>
        /// <param name="entity">The entity.</param>
       
        void Saved(T entity);
    }
}
