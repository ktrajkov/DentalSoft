namespace DentalSoft.Services.Patients
{
    using DentalSoft.Data.Contracts.AdditionalInfoes;
    using DentalSoft.Data.Contracts.Patientes;
    using DentalSoft.Services.Contracts;
    using System.Collections;
    using System.Collections.Generic;

    public interface IPatientsDataProvider : IService
    {
        PatientModel GetPatient (int? id);

        PatientPlanModel GetPatientPlan(int id);

        IEnumerable<AdditionalInfoModel> UpdateDeseases(DeseasesUpdateModel model);
    }
}
