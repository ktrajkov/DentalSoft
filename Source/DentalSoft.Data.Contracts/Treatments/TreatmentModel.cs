namespace DentalSoft.Data.Contracts.Treatments
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Contracts.Status.TeethStatus;
    using DentalSoft.Data.Models.Treatments;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TreatmentModel : PresentationModel, IMapFrom<Treatment>, IFormattable, IHaveCustomMappings
    {
        [Display(Name = "Treatment_Code", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "TreatmentModel_CodeRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        public string Code { get; set; }

        [Display(Name = "Treatment_Description", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "TreatmentModel_DescriptionRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(250)]
        public string Description { get; set; }

        [Display(Name = "Treatment_Price", ResourceType = typeof(Resource))]
        public decimal Price { get; set; }

        [Display(Name = "Treatment_IsVisible", ResourceType = typeof(Resource))]
        public bool IsVisible { get; set; }

        public int DiagnosisId { get; set; }

        [Display(Name = "Treatment_Visualization", ResourceType = typeof(Resource))]
        public ToothEditorModel ToothEditorModel { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Code + "-" + Description;
        }       

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Treatment, TreatmentModel>()
               .ForMember(dest => dest.ToothEditorModel, opt => opt.MapFrom(src => src.ToothChart));

            configuration.CreateMap<TreatmentModel, Treatment>()
               .ForMember(dest => dest.ToothChart, opt => opt.MapFrom(src => src.ToothEditorModel));
        }
    }
}
