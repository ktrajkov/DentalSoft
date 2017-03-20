namespace DentalSoft.Data.Models.Diseases
{
    using System.Collections.Generic;
    using DentalSoft.Data.Models.Status;

    public class DeseaseCategory : DeletableEntity
    {
        public DeseaseCategory()
        {
            this.deseases = new HashSet<Desease>();
        }
        
        public string Name { get; set; }

        public virtual int StatusId { get; set; }

        public virtual Status Status { get; set; }

        public virtual ICollection<Desease> Deseases
        {
            get { return this.deseases; }
            set { this.deseases = value; }
        }

        private ICollection<Desease> deseases { get; set; }
    }
}
