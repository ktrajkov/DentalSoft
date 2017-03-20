namespace DentalSoft.Data.Services.Interfaces
{
    // Summary:
    //     The interface for domain contract filled event.
    //
    // Type parameters:
    //   T:
    //     The entity type.
    //
    //   TContract:
    //     The contract type.
    public interface IContractFilled<T, TContract>
    {
        // Summary:
        //     Handles the contract filled event.
        //
        // Parameters:
        //   entity:
        //     The entity.
        //
        //   contract:
        //     The contract.
        //
        void ContractFilled(T entity, TContract contract);
    }
}
