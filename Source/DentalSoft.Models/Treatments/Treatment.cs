namespace DentalSoft.Data.Models.Treatments
{
    using DentalSoft.Data.Models.Diagnoses;
    using DentalSoft.Data.Models.Status.TeethStatus;
    using System.ComponentModel.DataAnnotations;

    public class Treatment : DeletableEntity
    {

       // [Required]
        [MaxLength(25)]
        public string Code { get; set; }

        [Required]
        [MaxLength(250)]
        public string  Description { get; set; }

        public decimal  Price { get; set; }

        public bool IsVisible { get; set; }

        public int DiagnosisId { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }

        public int? ToothChartId { get; set; }

        public virtual ToothChart ToothChart { get; set; }

    }
}
