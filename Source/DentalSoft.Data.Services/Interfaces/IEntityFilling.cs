namespace DentalSoft.Data.Services.Interfaces
{
    /// <summary>
    /// Handles the entity filling event.
    /// </summary>  
    /// <param name="TContract">The contract.</param>
    ///<param name="TEntity">The entity.</param>
    /// 
    public interface IEntityFilling<TContract, TEntity>
    {
        void Filling(TContract contract, TEntity entity);
    
    }
}
