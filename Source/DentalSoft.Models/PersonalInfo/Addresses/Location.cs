namespace DentalSoft.Data.Models.PersonalInfo.Addresses
{
    using System.ComponentModel.DataAnnotations;

    public class Location :DeletableEntity, IListEnitty
    {
        [MaxLength(50)]
        [Required]
        public string  Name { get; set; }

        public int MunicipalityId { get; set; }

        public virtual Municipality Municipality { get; set; }
    }
}
