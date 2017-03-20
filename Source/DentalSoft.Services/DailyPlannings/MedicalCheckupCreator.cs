namespace DentalSoft.Services.DailyPlannings
{
    using DentalSoft.Data.Models.DailyPlannings;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.Treatments;
    using DentalSoft.Data.Services;
    using System;

    public class MedicalCheckupCreator : IMedicalCheckupCreator
    {
        public void CheckAndCreate(Operation operation)
        {
            if (operation.TreatmentId != null && operation.TreatmentDateTime != null && operation.TreatmentDateTime > DateTime.Now.AddDays(-1))
            {
                var treatment = RepositoryManager.GetRepositoryForEntity<Treatment>().GetById(operation.TreatmentId);

                if (treatment.Diagnosis.Code == diagnosisCode && (treatment.Code == tretmentCodeForPP || treatment.Code == tretmentCodeForCompletedP))
                {
                    var patient = RepositoryManager.GetRepositoryForEntity<Patient>().GetById(operation.PatientId);
                    var startDate = operation.TreatmentDateTime.Value.AddMonths(patient.PersonalData.NextMedicalCheckUp != null ?
                        patient.PersonalData.NextMedicalCheckUp.Value : nextMedicalCheckupByDefault);
                    if (startDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        startDate = startDate.AddDays(2);
                    }
                    else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        startDate = startDate.AddDays(1);
                    }

                    var nextPlanningItem = new PlanningItem
                    {
                        Title = Strings.MedicalCheckup,
                        PatientId = operation.PatientId,
                        Status = StatusType.Unbooked,
                        Start = startDate.Date.Add(new TimeSpan(startHour, startMinutes, 0)),
                        End = startDate.Date.Add(new TimeSpan(endHour, 0, 0)),
                        DentistId = patient.PersonalData.DentistId
                    };
                    RepositoryManager.GetRepositoryForEntity<PlanningItem>().Add(nextPlanningItem);
                }
            }
        }

        #region Private Members
        //Codes for detecting the next medical check-up
        private const string diagnosisCode = "C01";
        private const string tretmentCodeForPP = "03";
        private const string tretmentCodeForCompletedP = "04";

        private const int nextMedicalCheckupByDefault = 6; //in months
        private const int startHour = 8;
        private const int startMinutes = 30;
        private const int endHour = 9;
        private const int duration = 30;
        #endregion
    }
}
