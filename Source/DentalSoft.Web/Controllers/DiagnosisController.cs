namespace DentalSoft.Web.Controllers
{
    using System.Web.Mvc;
    using DentalSoft.Web.Controllers.Base;
    using DentalSoft.Data.Models.Diagnoses;
    using DentalSoft.Data.Contracts;
    using DentalSoft.Data.Contracts.Diagnoses;

    public class DiagnosisController : EntityController<DiagnosisModel,Diagnosis,EntityFilter>
    {
        // GET: Diagnosis
        public ActionResult Index()
        {
            return View();
        }
    }
}