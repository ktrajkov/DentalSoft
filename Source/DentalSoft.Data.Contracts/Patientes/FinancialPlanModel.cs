namespace DentalSoft.Data.Contracts.Patientes
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.FinancialPlan;

    public class FinancialPlanModel : PresentationModel, IMapFrom<FinancialPlan>
    {
        public string  Text { get; set; }
    }
}
