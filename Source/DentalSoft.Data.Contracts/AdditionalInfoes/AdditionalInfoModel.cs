namespace DentalSoft.Data.Contracts.AdditionalInfoes
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.AdditionalInfoes;

    public class AdditionalInfoModel : PresentationModel, IMapFrom<AdditionalInfo>
    {
        public string Info { get; set; }

        public  int? PatientId { get; set; }

        public  int? StatusId { get; set; }

        public  int? DeseaseId { get; set; }
    }
}
