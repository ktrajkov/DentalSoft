namespace DentalSoft.Data.Models.PersonalInfo
{
    using DentalSoft.Data.Models;

    using System.ComponentModel.DataAnnotations;

    public class Telephone : DeletableEntity
    {
        [MaxLength(20)]
        public string Number { get; set; }
    }
}
