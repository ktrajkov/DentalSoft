namespace DentalSoft.Data.Models.AdditionalInfoes
{
    using DentalSoft.Data.Models.Diseases;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.Status;

    public class AdditionalInfo : DeletableEntity
    {
        public string Info { get; set; }

        public virtual int? PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual int? StatusId { get; set; }

        public virtual Status Status { get; set; }

        public virtual int? DeseaseCategoryId { get; set; }

        public virtual DeseaseCategory DeseaseCategory { get; set; }

        public virtual int? DeseaseId { get; set; }

        public virtual Desease Desease { get; set; }
    }
}
