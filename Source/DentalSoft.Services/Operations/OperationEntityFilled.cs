namespace DentalSoft.Services.Operations
{
    using DentalSoft.Data.Contracts.Operation;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Teeths;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using System.Linq;

    public class OperationEntityFilled : IEntityFilled<OperationModel, Operation>
    {
        public void Filled(OperationModel contract, Operation entity)
        {
            entity.Teeth.Clear();
            if (contract.Teeth != null)
            {
                var persister = RepositoryManager.GetRepositoryForEntity<Tooth>();

                foreach (var tooth in contract.Teeth)
                {
                    var toothEntity = persister.All().Where(x => x.Number == tooth.Number).FirstOrDefault();
                    entity.Teeth.Add(toothEntity);
                }
                persister.SaveChanges();
            }    
        }
    }
}
