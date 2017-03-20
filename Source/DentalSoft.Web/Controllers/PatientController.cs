namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts.Patientes;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Services;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;
    using System.Linq;
    using DentalSoft.Data.Contracts.Deseases;
    using DentalSoft.Services.Patients;

    public class PatientController : EntityController<PatientModel, Patient, PatientFilter>
    {
        public PatientController(IPatientsDataProvider patientDataProvider)
        {
            this.patientDataProvider = patientDataProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int? id)
        {
            return View("Edit", this.patientDataProvider.GetPatient(id));
        }

        public ActionResult GetPatientPlan(int id)
        {
            var result = this.patientDataProvider.GetPatientPlan(id);
            return JsonNet(result);

        }

        public ActionResult DeseasesUpdate(DeseasesUpdateModel model)
        {
            var result = this.patientDataProvider.UpdateDeseases(model);
            return JsonNet(result);
        }

        private readonly IPatientsDataProvider patientDataProvider;
    }
}