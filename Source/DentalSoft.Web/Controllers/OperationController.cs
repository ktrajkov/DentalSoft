namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Operation;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;

    public class OperationController : EntityController<OperationModel, Operation, OperationFilter>
    {
       
    }
}