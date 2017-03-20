namespace DentalSoft.Data.Contracts.Status
{
    using DentalSoft.Data.Contracts.Deseases;
    using DentalSoft.Data.Contracts.Mapping;
    using System.Collections.Generic;
    using DentalSoft.Data.Models.Status;

    public class StatusModel : PresentationModel, IMapFrom<Status>
    {
        public string  Name { get; set; }

        public bool HasAdditionalInfo { get; set; }

        public ICollection<DeseaseCategoryModel> DeseaseCategories { get; set; }
    }
}
