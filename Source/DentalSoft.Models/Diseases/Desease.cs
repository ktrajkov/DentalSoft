namespace DentalSoft.Data.Models.Diseases
{
    using DentalSoft.Data.Models.Patients;
    using Status;
    using System.Collections.Generic;

    public class Desease : DeletableEntity
    {
        public Desease()
        {
            this.patients = new HashSet<Patient>();
         }

        public string  Name { get; set; }

        public bool HasAdditionalInfo { get; set; }

        public virtual int DeseaseCategoryId { get; set; }

        public virtual DeseaseCategory DeseaseCategory { get; set; }

        public virtual ICollection<Patient> Patients
        {
            get { return this.patients; }
            set { this.patients = value; }
        }

        private ICollection<Patient> patients { get; set; }
    }
}
