namespace DentalSoft.Data.Contracts.Addresses
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using System;

    public class MunicipalityModel : PresentationModel, IMapFrom<Municipality>, IFormattable
    {
        public string Name { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Name;
        }
    }
}
