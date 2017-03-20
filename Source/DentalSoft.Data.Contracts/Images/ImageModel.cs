namespace DentalSoft.Data.Contracts.Images
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Images;

    public class ImageModel :PresentationModel, IMapFrom<Image>
    {
        public int? ToothNumber { get; set; }

        public ImageType Type { get; set; }

        public int PatientId { get; set; }

        public string ImageUrl { get; set; }
    }
}
