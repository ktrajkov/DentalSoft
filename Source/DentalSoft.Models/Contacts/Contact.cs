namespace DentalSoft.Data.Models.Contacts
{
    using DentalSoft.Data.Models.PersonalInfo;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Contact : DeletableEntity
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        [MaxLength(20)]
        public string Telephone { get; set; }

        [MaxLength(20)]
        public string MobileTelephonе { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int? PersonalDataId { get; set; }

        public virtual PersonalData PersonalData { get; set; }

        public ContactCategoryType ContactCategory { get; set; }
    }
}
