namespace DentalSoft.Data.Contracts.Dentists
{

    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Dentists;
    using System;

    public class DentistModel : PresentationModel, IMapFrom<Dentist>, IFormattable
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initials { get; set; }

        public string ImageUrl { get; set; }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Initials;
        }
    }
}
