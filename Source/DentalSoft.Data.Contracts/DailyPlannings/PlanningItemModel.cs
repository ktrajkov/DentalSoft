namespace DentalSoft.Data.Contracts.DailyPlannings
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.DailyPlannings;
    using System;
    using System.ComponentModel.DataAnnotations;
    using DentalSoft.Common.Extensions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class PlanningItemModel : PresentationModel, IMapFrom<PlanningItem>, IHaveCustomMappings
    {
        public string PatientName { get; set; }

        public string Phone { get; set; }

        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public StatusType Status { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public string StatusName
        {
            get { return Status.ToString(); }
        }

        public int? PatientId { get; set; }

        public int? DentistId { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<PlanningItem, PlanningItemModel>()
              .ForMember(p => p.PatientName, opt => opt.MapFrom(src => src.Patient.PersonalData != null?src.Patient.PersonalData.FirstName +
                  " " + src.Patient.PersonalData.LastName: null))
            .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.Patient.PersonalData.Contact.Telephone));
        }
    }
}
