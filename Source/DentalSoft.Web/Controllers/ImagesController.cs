namespace DentalSoft.Web.Controllers
{
    using DentalSoft.Data.Contracts.Images;
    using DentalSoft.Data.Models.Images;
    using DentalSoft.Data.Services;
    using DentalSoft.Web.Controllers.Base;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using DentalSoft.Services.Images;

    public class ImagesController : EntityController<ImageModel, Image, ImageFilter>
    {
        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, int patientId, ImageType imageType, int? toothNumber)
        {
            imageService.Save(files, patientId, imageType, toothNumber);
            return Content("");
        }

        public ActionResult Remove(int patientId, ImageType imageType, string fileName)
        {
            if (fileName != null)
            {
                imageService.Remove(patientId, imageType, fileName);
            }
            return Content("");
        }

        public ActionResult GetTeethWithImages(int patientId)
        {      
            return JsonNet(imageService.GetTeethWithImages(patientId));
        }

        #region Private Members
        private IImageService imageService;
        #endregion

    }
}