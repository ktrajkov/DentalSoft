namespace DentalSoft.Data.Models
{

    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class DeletableEntity : AuditInfo, IDeletableEntity
    {
        public virtual bool IsDeleted { get; set; }

        [Column(TypeName = "datetime2")]
        public virtual DateTime? DeletedOn { get; set; }
    }
}
