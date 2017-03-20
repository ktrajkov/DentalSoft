namespace DentalSoft.Data.Contracts.Patientes
{
    using System;
    using System.Linq;
    using AutoMapper;
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Contracts.Status;
    using DentalSoft.Data.Models.Patients;
    using DentalSoft.Data.Models.PersonalInfo;
    using DentalSoft.Data.Models.PersonalInfo.Addresses;
    using DentalSoft.Data.Models.Status;
    using System.Collections.Generic;
    using DentalSoft.Data.Contracts.Images;
    using DentalSoft.Data.Contracts.Operation;
    using DentalSoft.Data.Models.Images;
    using DentalSoft.Data.Contracts.Deseases;
    using DentalSoft.Data.Contracts.AdditionalInfoes;
    using System.ComponentModel.DataAnnotations;
    using DentalSoft.Data.Models.FinancialPlan;

    public class PatientModel : PresentationModel, IMapFrom<Patient>, IHaveCustomMappings, IFormattable
    {
        public string FirstName { get; set; }

        public bool HasIntraoralImages { get; set; }

        public bool HasExtraoralImages { get; set; }

        public bool HasXrayImages { get; set; }

        public bool HasOPGImages { get; set; }

        public bool HasCTImages { get; set; }

        public FinancialPlanModel  FinancialPlanModel { get; set; }

        public PersonalDataModel PersonalDataModel { get; set; }

        public ICollection<DeseaseModel> Deseases { get; set; }

        public ICollection<AdditionalInfoModel> AdditionalInfoes { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Patient, PatientModel>()
                .ForMember(dest => dest.PersonalDataModel, opt => opt.MapFrom(src => src.PersonalData))
                     .ForMember(dest => dest.FinancialPlanModel, opt => opt.MapFrom(src => src.FinancialPlan))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.PersonalData.FirstName))
                .ForMember(dest => dest.HasIntraoralImages, opt => opt.MapFrom(src => src.Images.Any(x => x.Type == ImageType.Intraoral)))
                .ForMember(dest => dest.HasExtraoralImages, opt => opt.MapFrom(src => src.Images.Any(x => x.Type == ImageType.Extraoral)))
                .ForMember(dest => dest.HasXrayImages, opt => opt.MapFrom(src => src.Images.Any(x => x.Type == ImageType.Xray)))
                .ForMember(dest => dest.HasOPGImages, opt => opt.MapFrom(src => src.Images.Any(x => x.Type == ImageType.OPG)))
                .ForMember(dest => dest.HasCTImages, opt => opt.MapFrom(src => src.Images.Any(x => x.Type == ImageType.CT)));

            configuration.CreateMap<PersonalData, PersonalDataModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));           

            configuration.CreateMap<PatientModel, Patient>()
                .ForMember(dest => dest.PersonalData, opt => opt.MapFrom(src => src.PersonalDataModel))
                .ForMember(dest => dest.PersonalDataId, opt => opt.MapFrom(src => src.PersonalDataModel.Id))
                 .ForMember(dest => dest.FinancialPlan, opt => opt.MapFrom(src => src.FinancialPlanModel))
                 .ForMember(dest => dest.Deseases, opt => opt.Ignore())
                 .ForMember(dest => dest.AdditionalInfoes, opt => opt.Ignore());

            configuration.CreateMap<PersonalDataModel, PersonalData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return PersonalDataModel.FirstName + " " + PersonalDataModel.LastName; 
        }
    }
}
