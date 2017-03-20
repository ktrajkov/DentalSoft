namespace DentalSoft.Data.Contracts.Contacts
{
    using DentalSoft.Data.Contracts.Dentists;
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Models.Dentists;
    using DentalSoft.Data.Models.PersonalInfo;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ContactModel : PresentationModel, IMapFrom<Contact>, IHaveCustomMappings
    {       
        [MaxLength(50)]
        [Display(Name = "Contacts_FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Contacts_SecondName", ResourceType = typeof(Resource))]
        public string SecondName { get; set; }
       
        [MaxLength(50)]
        [Display(Name = "Contacts_LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        public int? PersonalDataId { get; set; }

        [MaxLength(20)]
        [Display(Name = "Contacts_Telephone", ResourceType = typeof(Resource))]
        public string Telephone { get; set; }

        [MaxLength(20)]
        [Display(Name = "Contacts_MobileTelephone", ResourceType = typeof(Resource))]
        public string MobileTelephonе { get; set; }

        [MaxLength(20)]
        [Display(Name = "Contacts_Fax", ResourceType = typeof(Resource))]
        public string Fax { get; set; }

        [EmailAddress(ErrorMessageResourceName = "Contacts_EmailIsInvalidErrorMessage", ErrorMessageResourceType = typeof(Resource), ErrorMessage = null)]
        [Display(Name = "Contacts_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public ContactCategoryType ContactCategory { get; set; }

        [Display(Name = "Contacts_Dentist", ResourceType = typeof(Resource))]
        public string DentistInitials { get; set; }

        public int? DentistId { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ContactModel, Contact>()
               .ForMember(p => p.Id, opt => opt.MapFrom(model => model.Id)).ReverseMap();
            configuration.CreateMap<Contact, ContactModel>()
              .ForMember(p => p.Id, opt => opt.MapFrom(model => model.Id))             
              .ForMember(p => p.FirstName, opt => opt.MapFrom(model => model.PersonalData != null ? model.PersonalData.FirstName : model.FirstName))
              .ForMember(p => p.SecondName, opt => opt.MapFrom(model => model.PersonalData != null ? model.PersonalData.SecondName : model.SecondName))
              .ForMember(p => p.LastName, opt => opt.MapFrom(model => model.PersonalData != null ? model.PersonalData.LastName : model.LastName))
              .ForMember(p => p.DentistInitials, opt => opt.MapFrom(model => model.PersonalData != null ? model.PersonalData.Dentist.Initials : ""))
              .ForMember(p => p.DentistId, opt => opt.MapFrom(model => model.PersonalData != null ? (int?)model.PersonalData.DentistId : null));
        }
    }
}
