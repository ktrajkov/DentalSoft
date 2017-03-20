namespace DentalSoft.Data.Contracts.Patientes
{
    using DentalSoft.Common.Filters;
    using DentalSoft.Common.Mapping;

    public class PatientFilter
    {
        public int? Id { get; set; }

        [MapAssociation("PersonalData.FirstName")]
        [Equal]
        public string PersonalDataFirstName { get; set; }

        [MapAssociation("PersonalData.LastName")]
        [Equal]
        public string PersonalDataLastName { get; set; }
    }
}
