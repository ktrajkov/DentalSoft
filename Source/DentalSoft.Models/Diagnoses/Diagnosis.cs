namespace DentalSoft.Data.Models.Diagnoses
{
    using DentalSoft.Data.Models.Treatments;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DentalSoft.Data.Models.Status.TeethStatus;

    public class Diagnosis : DeletableEntity
    {
        public Diagnosis()
        {
            this.treatments = new HashSet<Treatment>();
        }


        [Required]
        [MaxLength(25)]
        public string Code { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descrtiption { get; set; }

        public bool IsVisible { get; set; }

        public int? ToothChartId { get; set; }

        public virtual ToothChart ToothChart { get; set; }

        public virtual ICollection<Treatment> Treatments
        {
            get { return this.treatments; }
            set { this.treatments = value; }
        }

        private ICollection<Treatment> treatments { get; set; }
    }
}
