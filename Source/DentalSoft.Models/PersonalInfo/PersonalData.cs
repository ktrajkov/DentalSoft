namespace DentalSoft.Data.Models.PersonalInfo
{
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PersonalData : DeletableEntity
    {
        [StringLength(10)]
        public string EGN { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { get; set; }

        [Range(0, 200)]
        public int? Age { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string SecondName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public int? AddressId { get; set; }

        public virtual Address Address { get; set; }    

        public Nationality Nationality { get; set; }

        public virtual int? ContactId { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual int DentistId { get; set; }

        public virtual Dentist Dentist { get; set; }

        [MaxLength(50)]
        public string GP { get; set; }

        [MaxLength(50)]
        public string LastDentist { get; set; }

        public HealthStatus HealthStatus { get; set; }

        public bool PrimaryPatient { get; set; }

        public bool HasDeciduousTeeth { get; set; }

        [Range(1, 12)]
        public int? NextMedicalCheckUp { get; set; }
    }
}
