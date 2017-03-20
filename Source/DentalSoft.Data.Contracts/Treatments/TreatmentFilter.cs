namespace DentalSoft.Data.Contracts.Treatments
{
    using DentalSoft.Common.Filters;

    public class TreatmentFilter : EntityFilter
    {
        public int? DiagnosisId { get; set; }

        [Equal]
        public string Description { get; set; }
    }
}
