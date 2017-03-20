namespace DentalSoft.Data.Contracts.Images
{
    using DentalSoft.Data.Models.Images;

    public class ImageFilter : EntityFilter
    {
        public int? ToothNumber { get; set; }

        public string ImageUrl { get; set; }

        public ImageType Type { get; set; }

        public int PatientId { get; set; }
    }
}
