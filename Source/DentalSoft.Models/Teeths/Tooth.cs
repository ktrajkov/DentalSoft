namespace DentalSoft.Data.Models.Teeths
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DentalSoft.Data.Models.Operation;

    public class Tooth : DeletableEntity
    {
        public Tooth()
        {
            this.oeprations = new HashSet<Operation>();
        }
        public int Number { get; set; }

        public virtual ICollection<Operation> Operations
        {
            get { return this.oeprations; }
            set { this.oeprations = value; }
        }

        private ICollection<Operation> oeprations { get; set; }
    }
}
