namespace DentalSoft.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class AuditInfo : BaseEntity, IAuditInfo
    {
        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Specifies whether or not the CreatedOn property should be automatically set.
        /// </summary>
        [NotMapped]
        public virtual bool PreserveCreatedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public virtual DateTime? ModifiedOn { get; set; }
    }
}
