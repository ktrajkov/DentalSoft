namespace DentalSoft.Data.Contracts.Patientes
{
    using AutoMapper;
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using DentalSoft.Data.Models.Contacts;
    using DentalSoft.Data.Contracts.Contacts;

    public class PersonalDataModel : PresentationModel, IMapFrom<PersonalData>, IHaveCustomMappings
    {
        //TODO: validate egn

        [StringLength(10)]
        [Display(Name = "PersonalData_EGN", ResourceType = typeof(Resource))]
        public string EGN { get; set; }

        //[Display(Name = "PersonalData_DateOfBirth", ResourceType = typeof(Resource))]
        //public DateTime? DateOfBirth { get; set; }

        [Range(0, 200)]
        [Display(Name = "PersonalData_Age", ResourceType = typeof(Resource))]
        public int? Age { get; set; }

        [Display(Name = "PersonalData_Gender", ResourceType = typeof(Resource))]
        public Gender Gender { get; set; }

        [Required(ErrorMessageResourceName = "PersonalDataModel_FirstNameRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(50)]
        [Display(Name = "PersonalData_FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "PersonalData_SecondName", ResourceType = typeof(Resource))]
        public string SecondName { get; set; }

        [Required(ErrorMessageResourceName = "PersonalDataModel_LastNameRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(50)]
        [Display(Name = "PersonalData_LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }


        public int? AddressId { get; set; }

        [Display(Name = "PersonalData_Region", ResourceType = typeof(Resource))]
        public int? AddressRegionId { get; set; }

        [Display(Name = "PersonalData_Municipality", ResourceType = typeof(Resource))]
        public int? AddressMunicipalityId { get; set; }

        [Display(Name = "PersonalData_Location", ResourceType = typeof(Resource))]
        public int? AddressLocationId { get; set; }

        [MaxLength(50)]
        [Display(Name = "PersonalData_Street", ResourceType = typeof(Resource))]
        public string AddressStreet { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_NumberStreet", ResourceType = typeof(Resource))]
        public string AddressNumberStreet { get; set; }

        [MaxLength(50)]
        [Display(Name = "PersonalData_ResidentialQuarter", ResourceType = typeof(Resource))]
        public string AddressResidentialQuarter { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_Building", ResourceType = typeof(Resource))]
        public string AddressBuilding { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_Entrance", ResourceType = typeof(Resource))]
        public string AddressEntrance { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_Floor", ResourceType = typeof(Resource))]
        public string AddressFloor { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_Apt", ResourceType = typeof(Resource))]
        public string AddressApt { get; set; }

        [MaxLength(10)]
        [Display(Name = "PersonalData_ZipCode", ResourceType = typeof(Resource))]
        public string AddressZipCode { get; set; }

        public int? ContactId { get; set; }


        [MaxLength(20)]
        [Display(Name = "PersonalData_Telephone", ResourceType = typeof(Resource))]
        public string Telephone { get; set; }

        [MaxLength(20)]
        [Display(Name = "PersonalData_MobileTelephone", ResourceType = typeof(Resource))]
        public string MobileTelephonе { get; set; }

        [MaxLength(20)]
        [Display(Name = "PersonalData_Fax", ResourceType = typeof(Resource))]
        public string Fax { get; set; }

        [EmailAddress(ErrorMessageResourceName = "PersonalDataModel_EmailIsInvalidErrorMessage", ErrorMessageResourceType = typeof(Resource), ErrorMessage = null)]
        [Display(Name = "PersonalData_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Display(Name = "PersonalData_Nationality", ResourceType = typeof(Resource))]
        public Nationality Nationality { get; set; }

        [Display(Name = "PersonalData_Dentist", ResourceType = typeof(Resource))]
        public string DentistInitials { get; set; }

        [Required(ErrorMessageResourceName = "PersonalDataModel_DentistRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "PersonalData_Dentist", ResourceType = typeof(Resource))]
        public int DentistId { get; set; }

        [MaxLength(50)]
        [Display(Name = "PersonalData_LastDentist", ResourceType = typeof(Resource))]
        public string LastDentist { get; set; }

        [MaxLength(50)]
        [Display(Name = "PersonalData_GP", ResourceType = typeof(Resource))]
        public string GP { get; set; }

        [Display(Name = "PersonalData_HealthStatus", ResourceType = typeof(Resource))]
        public HealthStatus HealthStatus { get; set; }

        [Display(Name = "PersonalData_PrimaryPatient", ResourceType = typeof(Resource))]
        public bool PrimaryPatient { get; set; }
      
        public bool HasDeciduousTeeth { get; set; }

        [Range(1, 12)]
        [Display(Name = "PersonalData_NextMedicalCheckUp", ResourceType = typeof(Resource))]
        public int? NextMedicalCheckUp { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PersonalDataModel, Address>();
            configuration.CreateMap<PersonalDataModel, Contact>()
                .ForMember(p => p.ContactCategory, opt => opt.MapFrom(src => ContactCategoryType.Patients))
                .ForMember(p => p.Id, opt => opt.MapFrom(model => model.ContactId))
                .ForMember(p => p.FirstName, opt => opt.Ignore())
                .ForMember(p => p.SecondName, opt => opt.Ignore())
                .ForMember(p => p.LastName, opt => opt.Ignore());
            configuration.CreateMap<PersonalDataModel, PersonalData>()
               .ForMember(p => p.Address, opt => opt.MapFrom(model => model))
               .ForMember(p => p.Contact, opt => opt.MapFrom(model => model))
               .AfterMap((s, d) => { CheckForNullProperties<PersonalData>(d, d.Address); d.Contact.PersonalDataId = s.Id; })
               .ReverseMap();
            configuration.CreateMap<PersonalData, PersonalDataModel>()
              .ForMember(p => p.Telephone, opt => opt.MapFrom(src => src.Contact.Telephone))
              .ForMember(p => p.MobileTelephonе, opt => opt.MapFrom(src => src.Contact.MobileTelephonе))
              .ForMember(p => p.Fax, opt => opt.MapFrom(src => src.Contact.Fax))
              .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Contact.Email));
        }

        //TODO: Move method into separate file
        private static void CheckForNullProperties<TSource>(TSource source, object checkableProp)
        {
            if (checkableProp != null)
            {
                var checkablePropName = checkableProp.GetType().Name;
                var prop = source.GetType().GetProperty(checkablePropName);
                var propValue = prop.GetValue(source, null);
                var nestedProperties = prop.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (!nestedProperties.Any(p => p.GetValue(propValue) != null))
                {
                    source.GetType().GetProperty(checkablePropName).SetValue(source, null);
                }
            }
        }
    }
}
