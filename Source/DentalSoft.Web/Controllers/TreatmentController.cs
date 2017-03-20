namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Treatments;
    using DentalSoft.Data.Models.Treatments;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;

    public class TreatmentController : EntityController<TreatmentModel,Treatment,TreatmentFilter>
    {
        // GET: Treatment
        public ActionResult Index()
        {
            return View();
        }
    }
}