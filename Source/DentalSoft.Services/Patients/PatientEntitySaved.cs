namespace DentalSoft.Services.Patients
{
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using DentalSoft.Services.Operations;

    public class PatientEntitySaved : IEntitySaved<Patient>
    {
        public PatientEntitySaved(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public void Saved(Patient entity)
        {
            if (entity.Id == 0)
            {
                RepositoryManager.GetRepositoryForEntity<Patient>().SaveChanges();
                entity.PersonalData.Contact.PersonalDataId = entity.PersonalDataId;
                entity.PersonalData.Contact.PersonalData = entity.PersonalData;
                if (!entity.PersonalData.HasDeciduousTeeth)
                {
                    operationProvider.RemoveDeciduousTeeth(entity, null);
                }

            }

        }

        private IOperationProvider operationProvider { get; set; }
    }
}
