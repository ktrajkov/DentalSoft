namespace DentalSoft.Data.Models.Status
{
    using DentalSoft.Data.Models.Diseases;
    using System.Collections.Generic;

    public class Status : DeletableEntity
    {
        public Status()
        {
            this.deseaseCategories = new HashSet<DeseaseCategory>();
        }

        public string Name { get; set; }

        public bool HasAdditionalInfo { get; set; }

        public virtual ICollection<DeseaseCategory> DeseaseCategories
        {
            get { return this.deseaseCategories; }
            set { this.deseaseCategories = value; }
        }

        private ICollection<DeseaseCategory> deseaseCategories { get; set; }
    }
}
