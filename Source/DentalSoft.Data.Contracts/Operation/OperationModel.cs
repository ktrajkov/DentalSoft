namespace DentalSoft.Data.Contracts.Operation
{
    using DentalSoft.Data.Contracts.Status.TeethStatus;
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Common.Extensions;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using DentalSoft.Data.Contracts.Teeth;

    public class OperationModel : PresentationModel, IMapFrom<Operation>, IHaveCustomMappings
    {
        public int PatientId { get; set; }

        [Display(Name = "Operation_DiagnosisDateTime", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        public DateTime? DiagnosisDateTime { get; set; }

        [Display(Name = "Operation_TreatmentDateTime", ResourceType = typeof(Resource))]
        public DateTime? TreatmentDateTime { get; set; }

        [Display(Name = "Operation_AdditionalInfo", ResourceType = typeof(Resource))]
        public string AdditionalInfo { get; set; }

        [Display(Name = "Operation_Quantity", ResourceType = typeof(Resource))]
        public int Quantity { get; set; }

        public string DiagnosisName { get; set; }

        [Required(ErrorMessageResourceName = "Operation_DiagnosisRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Operation_Diagnosis", ResourceType = typeof(Resource))]
        public int DiagnosisId { get; set; }

        public ToothChartModel DiagnosisToothChart { get; set; }

        public string TreatmentName { get; set; }

        [Display(Name = "Operation_Treatment", ResourceType = typeof(Resource))]
        public int? TreatmentId { get; set; }

        public ToothChartModel TreatmentToothChart { get; set; }

        [Display(Name = "Operation_Position", ResourceType = typeof(Resource))]
        public PositionType? Position { get; set; }

        public string PositionText
        {
            get
            {
                var positionText = this.Position.GetDescription();
                return positionText != null ? positionText : "";
            }
        }

        public string DentistName { get; set; }

        [Required(ErrorMessageResourceName = "Operation_DentistRequiredErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Operation_Dentist", ResourceType = typeof(Resource))]
        public int DentistId { get; set; }

        [Display(Name = "Operation_Teeth", ResourceType = typeof(Resource))]
        public ICollection<ToothModel> Teeth { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Operation, OperationModel>()
                .ForMember(dest => dest.DiagnosisName, opt => opt.MapFrom(src => src.Diagnosis.Code + " - " + src.Diagnosis.Descrtiption))
                .ForMember(dest => dest.DiagnosisToothChart, opt => opt.MapFrom(src => src.Diagnosis.ToothChart))
                .ForMember(dest => dest.TreatmentToothChart, opt => opt.MapFrom(src => src.Treatment.ToothChart))
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment.Code != null ? src.Treatment.Code + " - " + src.Treatment.Description : ""))
                .ForMember(dest => dest.DentistName, opt => opt.MapFrom(src => src.Dentist.Initials));


            configuration.CreateMap<OperationModel, Operation>()
                 .ForMember(dest => dest.Teeth, opt => opt.Ignore());
        }
    }
}
