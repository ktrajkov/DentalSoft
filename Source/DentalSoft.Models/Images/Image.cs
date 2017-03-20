using DentalSoft.Data.Models.Patients;
namespace DentalSoft.Data.Models.Images
{
    public class Image : DeletableEntity
    {
        public int? ToothNumber { get; set; }

        public string ImageUrl { get; set; }

        public ImageType Type { get; set; }

        public virtual int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

    }
}
