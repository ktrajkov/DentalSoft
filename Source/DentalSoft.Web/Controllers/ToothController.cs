namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Teeth;
    using DentalSoft.Data.Models.Teeths;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;

    public class ToothController : EntityController<ToothModel, Tooth, EntityFilter>
    {
        // GET: Tooth
        public ActionResult Index()
        {
            return View();
        }
    }
}