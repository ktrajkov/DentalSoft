namespace DentalSoft.Services.Images
{
    using DentalSoft.Data.Models.Images;
    using DentalSoft.Services.Contracts;
    using System.Collections.Generic;
    using System.Web;

    public interface IImageService : IService
    {
        void Save(IEnumerable<HttpPostedFileBase> files, int patientId, ImageType imageType, int? toothNumber);
        void Remove(int patientId, ImageType imageType, string fileName);
        object GetTeethWithImages(int patientId);
    }
}
