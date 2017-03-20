namespace DentalSoft.Data.Contracts.DailyPlannings
{
    using DentalSoft.Common.Filters;
    using DentalSoft.Common.Mapping;
    using DentalSoft.Data.Models.DailyPlannings;
    using System;

    public class PlanningItemFilter : EntityFilter
    {
        public int? Id { get; set; }

        [GreaterThanOrEqual]
        public DateTime? Start { get; set; }

        [LessThanOrEqual]
        public DateTime? End { get; set; }

        [In]
        [MapAssociation("Dentist.Id")]
        public int[] DentistIds { get; set; }

        public int? PatientId { get; set; }

        public StatusType? Status { get; set; }
    }
}
