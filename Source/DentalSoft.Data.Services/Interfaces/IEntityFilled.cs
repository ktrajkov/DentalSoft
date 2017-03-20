namespace DentalSoft.Data.Services.Interfaces
{
    // Summary:
    //     The interface for domain entity saved event.
    //
    // Type parameters:
    //   T:
    //     The entity type.
    //
    //   TContract:
    //     The contract type.
    public interface IEntityFilled<TContract, TEntity>
    {
        // Summary:
        //     Handles the entity filled event.
        //
        // Parameters:
        //   entity:
        //     The entity.
        //
        //   contract:
        //     The contract.
        //      
        void Filled(TContract contract, TEntity entity);
    }
}
