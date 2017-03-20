namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts.Addresses;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using DentalSoft.Web.Controllers.Base;

    public class LocationController :EntityController<LocationModel,Location,LocationFilter>
    {      
    }
}