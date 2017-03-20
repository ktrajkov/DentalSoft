using DentalSoft.Data.Contracts;
using DentalSoft.Data.Contracts.Deseases;
using DentalSoft.Data.Models.Diseases;
using DentalSoft.Web.Controllers.Base;

namespace DentalSoft.Web.Controllers
{
    public class DeseaseController : EntityController<DeseaseModel, Desease, DeseaseFilter>
    {      
    }
}