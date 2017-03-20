namespace DentalSoft.Services.DailyPlannings
{
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Services.Contracts;

    public interface IMedicalCheckupCreator : IService
    {
        void CheckAndCreate(Operation operation);
    }
}
