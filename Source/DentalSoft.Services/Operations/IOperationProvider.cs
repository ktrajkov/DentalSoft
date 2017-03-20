namespace DentalSoft.Services.Operations
{
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Services.Contracts;
    using System;

    public interface IOperationProvider : IService
    {
        void RemoveDeciduousTeeth(Patient patient, DateTime? diagnosisDate);
    }
}
