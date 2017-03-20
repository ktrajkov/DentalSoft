namespace DentalSoft.Web.Controllers.Phonebook
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Contacts;
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;

    public class ContactController : EntityController<ContactModel, Contact, ContactFilter>
    {
    }
}