namespace DentalSoft.Services.Patients
{
    using DentalSoft.Common;
    using DentalSoft.Data.Contracts.AdditionalInfoes;
    using DentalSoft.Data.Contracts.Patientes;
    using DentalSoft.Data.Models.AdditionalInfoes;
    using DentalSoft.Data.Models.Diseases;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Services;
    using System.Collections.Generic;
    using System.Linq;


    public class PatientDataProvider : IPatientsDataProvider
    {
        public IEnumerable<AdditionalInfoModel> UpdateDeseases(DeseasesUpdateModel model)
        {
            ExceptionUtil.NotNull(model, "model");
            var patientRepository = RepositoryManager.GetRepositoryForEntity<Patient>();
            var deseaseRepository = RepositoryManager.GetRepositoryForEntity<Desease>();
            var additionalInfoRepository = RepositoryManager.GetRepository<AdditionalInfoModel, AdditionalInfo>();
            var patient = patientRepository.GetById(model.PatientId);
            patient.Deseases.Where(x => x.DeseaseCategory.Status.Name == model.StatusName)
                .ToList().ForEach(x => patient.Deseases.Remove(x));

            if (model.DeseasesIds != null)
            {
                foreach (var deseaseId in model.DeseasesIds)
                {
                    var deseaseEntity = deseaseRepository.GetById(deseaseId);
                    patient.Deseases.Add(deseaseEntity);
                }
            }

            patientRepository.SaveChanges();

            List<AdditionalInfoModel> result = new List<AdditionalInfoModel>();

            if (model.AdditionalInfoes != null)
            {
                foreach (var info in model.AdditionalInfoes)
                {
                    additionalInfoRepository.Save(info);
                    RepositoryManager.Commit();
                    result.Add(additionalInfoRepository.Load());
                }         
            } 
            return result;
        }

        public PatientModel GetPatient(int? id)
        {
            var persister = RepositoryManager.GetRepository<PatientModel, Patient>();
            return persister.GetByIdToModel(id);
        }

        public PatientPlanModel GetPatientPlan(int id)
        {
            var persister = RepositoryManager.GetRepository<PatientPlanModel, Patient>();
            return persister.GetByIdToModel(id);
        }
    }
}
