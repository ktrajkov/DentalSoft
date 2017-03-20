namespace DentalSoft.Web.Controllers.Plan
{
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.DailyPlannings;
    using DentalSoft.Data.Models.DailyPlannings;
    using DentalSoft.Web.Controllers.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;


    public class PlanController : EntityController<PlanningItemModel, PlanningItem, PlanningItemFilter>
    {
        // GET: Plan
        public ActionResult Index()
        {
            return View();
        }
    }
}