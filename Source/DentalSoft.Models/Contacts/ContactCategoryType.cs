namespace DentalSoft.Data.Models.Contacts
{
    using DentalSoft.Common.CustomAttributes;

    public enum ContactCategoryType
    {
        [LocalizedDescription("ContactCategoryType_Patients", typeof(Strings))]
        Patients,

        [LocalizedDescription("ContactCategoryType_DentalTechnicians", typeof(Strings))]
        DentalTechnicians,

        [LocalizedDescription("ContactCategoryType_Technicians", typeof(Strings))]
        Technicians,

        [LocalizedDescription("ContactCategoryType_DentalDepots", typeof(Strings))]
        DentalDepots,

        [LocalizedDescription("ContactCategoryType_Distributors", typeof(Strings))]
        Distributors,

        [LocalizedDescription("ContactCategoryType_Colleagues", typeof(Strings))]
        Colleagues,

        [LocalizedDescription("ContactCategoryType_OrdinaryDoctors", typeof(Strings))]
        OrdinaryDoctors,

        [LocalizedDescription("ContactCategoryType_Others", typeof(Strings))]
        Others,
    }
}
