namespace DentalSoft.Data.Contracts.Diagnoses
{
    using DentalSoft.Common.Filters;

    public class DiagnosisFilter : EntityFilter
    {
        [Equal]
        public string Code { get; set; }
    }
}
