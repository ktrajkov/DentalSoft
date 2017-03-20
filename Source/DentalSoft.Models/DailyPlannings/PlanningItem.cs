namespace DentalSoft.Data.Models.DailyPlannings
{
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.Patients;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PlanningItem : DeletableEntity
    {
        public string Title { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Start { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime End { get; set; }

        public StatusType Status { get; set; }

        public string  Description { get; set; }

        public virtual int? PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual int? DentistId { get; set; }

        public virtual Dentist Dentist { get; set; }
    }
}
