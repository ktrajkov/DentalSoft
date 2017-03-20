namespace DentalSoft.Services.Patients
{
    using DentalSoft.Data.Contracts.Patientes;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Exceptions;
    using DentalSoft.Data.Services.Interfaces;
    using System.Linq;

    public class PatientCheckForUniqueness : IEntityCheckForUniqueness<Patient>
    {
        public void CheckForUniqueness(Patient entity)
        {
            var persister = RepositoryManager.GetRepositoryForEntity<Patient>();
            var filter = new PatientFilter
            {
                PersonalDataFirstName = entity.PersonalData.FirstName,
                PersonalDataLastName = entity.PersonalData.LastName
            };           
            var result = persister.All<PatientFilter>(filter);
            if (result.Any(x=>x.Id != entity.Id))
            {
                throw new NonUniqueEntityException(Strings.PersonalDataCheckForUniqueness_FirstNameAndLastNameShouldBeUnique);
            }

        }
    }
}
