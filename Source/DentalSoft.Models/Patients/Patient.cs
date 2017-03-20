namespace DentalSoft.Data.Models.Patients
{
    using DentalSoft.Data.Models;
    using DentalSoft.Data.Models.Images;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Models.Status;
    using DentalSoft.Data.Models.Operation;
    using System.Collections.Generic;
    using DentalSoft.Data.Models.Diseases;
    using DentalSoft.Data.Models.AdditionalInfoes;
    using DentalSoft.Data.Models.FinancialPlan;

    public class Patient : DeletableEntity
    {
        public Patient()
        {
            this.deseases = new HashSet<Desease>();
            this.additionalInfoes = new HashSet<AdditionalInfo>();
            this.operation = new HashSet<Operation>();
            this.images = new HashSet<Image>();
        }

        public int? PersonalDataId { get; set; }

        public virtual PersonalData PersonalData { get; set; }

        public int? FinancialPlanId { get; set; }

        public virtual FinancialPlan FinancialPlan { get; set; }    

        public virtual ICollection<Desease> Deseases
        {
            get { return this.deseases; }
            set { this.deseases = value; }
        }

        public virtual ICollection<AdditionalInfo> AdditionalInfoes
        {
            get { return this.additionalInfoes; }
            set { this.additionalInfoes = value; }
        }

        public virtual ICollection<Operation> Operations
        {
            get { return this.operation; }
            set { this.operation = value; }
        }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        private ICollection<Desease> deseases { get; set; }

        private ICollection<AdditionalInfo> additionalInfoes { get; set; }

        private ICollection<Operation> operation { get; set; }

        private ICollection<Image> images { get; set; }
    }
}
