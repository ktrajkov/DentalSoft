namespace DentalSoft.Data.Contracts.Contacts
{
    using DentalSoft.Data.Models.Contacts;

    public class ContactFilter : EntityFilter
    {
        public ContactCategoryType ContactCategory { get; set; }
    }
}
