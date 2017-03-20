namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Dentists;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Web.Controllers.Base;

    public class DentistController : EntityController<DentistModel, Dentist, EntityFilter>
    {       
    }
}