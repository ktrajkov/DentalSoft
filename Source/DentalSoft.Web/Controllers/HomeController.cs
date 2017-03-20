namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts.Dentists;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Services;
    using DentalSoft.Services.Operations;
    using DentalSoft.Web.Controllers.Base;
    using System.Web.Mvc;
    using System.Linq;

    public class HomeController : BaseController
    {   
        public HomeController(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public ActionResult Index()
        {
            //var patientRepository = RepositoryManager.GetRepositoryForEntity<Patient>();
            //var allPatients = patientRepository.All().ToList();
            //foreach (var patient in allPatients)
            //{
            //    operationProvider.RemoveDeciduousTeeth(patient, null);
            //}
            //patientRepository.SaveChanges();

            return View(RepositoryManager.GetRepository<DentistModel, Dentist>().AllToModel());
        }
    
public  IOperationProvider operationProvider { get; set; }}
}