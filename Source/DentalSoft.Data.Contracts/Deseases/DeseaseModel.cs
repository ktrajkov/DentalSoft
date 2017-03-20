namespace DentalSoft.Data.Contracts.Deseases
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Diseases;
    using System.Collections.Generic;

    public class DeseaseModel : PresentationModel, IMapFrom<Desease>
    {
        public string Name { get; set; }

        public bool HasAdditionalInfo { get; set; }
    }
}
    