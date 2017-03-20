namespace DentalSoft.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
