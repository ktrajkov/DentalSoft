namespace DentalSoft.Data.Contracts.Patientes
{
    using DentalSoft.Data.Contracts.AdditionalInfoes;
using DentalSoft.Data.Contracts.Deseases;
using System.Collections.Generic;

    public class DeseasesUpdateModel : PresentationModel
    {
        public int PatientId { get; set; }

        public string  StatusName { get; set; }

        public ICollection<AdditionalInfoModel> AdditionalInfoes { get; set; }

        public ICollection<int> DeseasesIds { get; set; }
    }
}
