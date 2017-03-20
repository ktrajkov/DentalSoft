namespace DentalSoft.Data.Contracts.Diagnoses
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Contracts.Treatments;
    using DentalSoft.Data.Models.Diagnoses;
    using DentalSoft.Data.Contracts.Status.TeethStatus;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;

    public class DiagnosisModel : PresentationModel, IMapFrom<Diagnosis>, IHaveCustomMappings, IFormattable
    {
        [Display(Name = "Diagnosis_Code", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "DiagnosisModel_CodeRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        public string Code { get; set; }

        [Display(Name = "Diagnosis_Description", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "DiagnosisModel_DescriptionRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        public string Descrtiption { get; set; }

        [Display(Name = "Diagnosis_IsVisible", ResourceType = typeof(Resource))]
        public bool IsVisible { get; set; }

        [Display(Name = "Diagnosis_Visualization", ResourceType = typeof(Resource))]
        public ToothEditorModel ToothEditorModel { get; set; }

        public ICollection<TreatmentModel> Treatsments { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Code + "-" + Descrtiption;
        }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Diagnosis, DiagnosisModel>()
                 .ForMember(dest => dest.ToothEditorModel, opt => opt.MapFrom(src => src.ToothChart));

            configuration.CreateMap<DiagnosisModel, Diagnosis>()
               .ForMember(dest => dest.ToothChart, opt => opt.MapFrom(src => src.ToothEditorModel));
        }

    }
}
