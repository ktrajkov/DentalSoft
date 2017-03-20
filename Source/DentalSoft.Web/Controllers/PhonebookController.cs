using DentalSoft.Data.Contracts.Contacts;
using DentalSoft.Data.Models.Contacts;
using DentalSoft.Data.Models.PersonalInfo;
using DentalSoft.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DentalSoft.Web.Controllers
{
    public class PhonebookController : Controller
    {
        // GET: Phonebook
        public ActionResult Index()
        {
            //var pdRep = RepositoryManager.GetRepositoryForEntity<PersonalData>();
            //var contantRep = RepositoryManager.GetRepository<ContactModel, Contact>();
            //var allPd = pdRep.All().ToList();
            //foreach (var pd in allPd)
            //{
            //    var contact = new ContactModel
            //         {
            //             Email = pd.Email,
            //             Telephone = pd.Telephone != null ? pd.Telephone.Number : "",
            //             MobileTelephone = pd.MobileTelephone != null ? pd.MobileTelephone.Number : "",
            //             Fax = pd.Fax != null ? pd.Fax.Number : "",
            //             CategoryId = 1,
            //             PersonalDataId = pd.Id,
            //         };
            //    contantRep.Save(contact);
            //    contantRep.SaveChanges();
            //    var newContactModel = contantRep.Load();
            //    pd.ContactId = newContactModel.Id;
            //}
            //pdRep.SaveChanges();




            return View();
        }
    }
}