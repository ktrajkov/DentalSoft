namespace DentalSoft.Data.Contracts.Status.TeethStatus
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Status.TeethStatus;
    using DentalSoft.Common.Extensions;

    public class ToothChartModel : PresentationModel, IMapFrom<ToothChart>, IHaveCustomMappings
    {
        public string Type { get; set; }

        public string Color { get; set; }


        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ToothChart, ToothChartModel>()
                .ForMember(p => p.Type, opt => opt.MapFrom(src => src.Type != null ? src.Type.ToString().ToLower() : ""))
                .ForMember(p => p.Color, opt => opt.MapFrom(src => src.Color != null ? src.Color.ToString().ToLower() : ""));
        }
    }
}
