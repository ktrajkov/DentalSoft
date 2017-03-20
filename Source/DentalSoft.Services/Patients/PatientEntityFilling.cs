
namespace DentalSoft.Services.Patients
{
    using DentalSoft.Data.Contracts.Patientes;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Services.Interfaces;
    using DentalSoft.Services.Operations;
    using System;

    public class PatientEntityFilling : IEntityFilling<PatientModel, Patient>
    {
        public PatientEntityFilling(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public void Filling(PatientModel contract, Patient entity)
        {
            if (entity.Id > 0 && contract.PersonalDataModel.HasDeciduousTeeth == false && entity.PersonalData.HasDeciduousTeeth == true)
            {
                this.operationProvider.RemoveDeciduousTeeth(entity, DateTime.Now);
            }
        }

        private IOperationProvider operationProvider { get; set; }
    }
}
