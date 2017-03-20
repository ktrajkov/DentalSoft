namespace DentalSoft.Services.Operations
{
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.Diagnoses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Data;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Teeths;
    using DentalSoft.Data.Services;

    public class OperationProvider : IOperationProvider
    {
        public void RemoveDeciduousTeeth(Patient patient,DateTime? diagnosisDate)
        {
            var removedDeciduousTeeth = patient.Operations.Where(o => o.Diagnosis.Code == extractioDiagnosisCodeFromOtherClinic 
                || o.Diagnosis.Code == extractioDiagnosisCode).SelectMany(x => x.Teeth).Select(t => t.Number);
            var availableDeciduousTeeth = deciduousTeethNumbers.Except(removedDeciduousTeeth).ToList();

            if (availableDeciduousTeeth.Count > 0)
            {
                var toothDataPersister = RepositoryManager.GetRepositoryForEntity<Tooth>();
                var diagnosisDataPersister = RepositoryManager.GetRepositoryForEntity<Diagnosis>();
                var extractioDiagnosisFromOtherClinic = diagnosisDataPersister.All()
                    .Where(x => x.Code == extractioDiagnosisCodeFromOtherClinic).FirstOrDefault();
                var newOperation = new Operation
                {
                    Diagnosis = extractioDiagnosisFromOtherClinic,
                    DiagnosisDateTime = diagnosisDate,
                    DentistId = patient.PersonalData.DentistId
                };

                foreach (var tooth in availableDeciduousTeeth)
                {
                    var toothEntity = toothDataPersister.All().Where(x => x.Number == tooth).FirstOrDefault();
                    newOperation.Teeth.Add(toothEntity);
                }

                patient.Operations.Add(newOperation);
            }
        }

        private readonly string extractioDiagnosisCode = "D01";
        private readonly string extractioDiagnosisCodeFromOtherClinic = "06";

        private readonly int[] deciduousTeethNumbers =new[] {55,54,53,52,51,61,62,63,64,65,75,
            74,73,72,71,81,82,83,84,85 };
    }
}
