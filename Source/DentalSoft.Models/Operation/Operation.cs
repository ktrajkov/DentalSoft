namespace DentalSoft.Data.Models.Operation
{
    using DentalSoft.Data.Models.Teeths;
    using DentalSoft.Data.Models.Diagnoses;
    using System;
    using DentalSoft.Data.Models.Treatments;
    using System.ComponentModel.DataAnnotations.Schema;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.Patients;
    using System.Collections.Generic;

    public class Operation : DeletableEntity
    {
        public Operation()
        {
            this.teeth = new HashSet<Tooth>();
        }

        [Column(TypeName = "datetime2")]
        public DateTime? DiagnosisDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TreatmentDateTime { get; set; }

        public string AdditionalInfo { get; set; }

        public int Quantity { get; set; }

        public virtual int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public int DiagnosisId { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }

        public virtual int? TreatmentId { get; set; }

        public virtual Treatment Treatment { get; set; }

        public PositionType? Position { get; set; }

        public virtual int DentistId { get; set; }

        public virtual Dentist Dentist { get; set; }

        public virtual ICollection<Tooth> Teeth
        {
            get { return this.teeth; }
            set { this.teeth = value; }
        }

        private ICollection<Tooth> teeth { get; set; }

    }
}
