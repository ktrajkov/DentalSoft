namespace DentalSoft.Services.Contacts
{
    using DentalSoft.Data.Contracts.Contacts;
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Interfaces;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Services.Patients;
    using DentalSoft.Data.Services.Exceptions;

    public class ContactEntityFilled : IEntityFilled<ContactModel, Contact>
    {
        public void Filled(ContactModel contract, Contact entity)
        {
            if (entity.ContactCategory == ContactCategoryType.Patients)
            {
                if (contract.FirstName == null)
                {
                    throw new RequiredFilledException(Strings.PersonalData_FirstNameIsRequired);
                }
                if (contract.LastName == null)
                {
                    throw new RequiredFilledException(Strings.PersonalData_LastNameIsRequired);
                }
                if (!contract.DentistId.HasValue)
                {
                    throw new RequiredFilledException(Strings.PersonalData_DentistIsRequired);
                }
                entity.FirstName = null;
                entity.SecondName = null;
                entity.LastName = null;

                if (entity.Id > 0)
                {
                    entity.PersonalData.FirstName = contract.FirstName;
                    entity.PersonalData.SecondName = contract.SecondName;
                    entity.PersonalData.LastName = contract.LastName;
                    entity.PersonalData.DentistId = contract.DentistId.Value;
                }
                else
                {
                    var patientRepository = RepositoryManager.GetRepositoryForEntity<Patient>();                  
                    var dentist = RepositoryManager.GetRepositoryForEntity<Dentist>().GetById(contract.DentistId.Value);
                    var newPatient = new Patient
                    {
                        PersonalData = new PersonalData
                        {
                            FirstName = contract.FirstName,
                            SecondName = contract.SecondName,
                            LastName = contract.LastName,
                            Contact = entity,
                            DentistId = contract.DentistId.Value,
                            Dentist = dentist,
                        }
                    };
                    var patientChecker = new PatientCheckForUniqueness();
                    patientChecker.CheckForUniqueness(newPatient);
                    patientRepository.Add(newPatient);
                    patientRepository.SaveChanges();
                    entity.PersonalDataId = newPatient.PersonalDataId;
                }
            }           
        }
    }
}
