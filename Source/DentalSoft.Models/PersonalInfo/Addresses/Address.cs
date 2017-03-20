namespace DentalSoft.Data.Models.PersonalInfo.Addresses
{
    using System.ComponentModel.DataAnnotations;

    public class Address : DeletableEntity
    {
        public int? RegionId { get; set; }

        public int? MunicipalityId { get; set; }

        public int? LocationId { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }

        [MaxLength(10)]
        public string NumberStreet { get; set; }

        [MaxLength(50)]
        public string ResidentialQuarter { get; set; }

        [MaxLength(10)]
        public string Building { get; set; }

        [MaxLength(10)]
        public string Entrance { get; set; }

        [MaxLength(10)]
        public string Floor { get; set; }

        [MaxLength(10)]
        public string Apt { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }
    }       
}
