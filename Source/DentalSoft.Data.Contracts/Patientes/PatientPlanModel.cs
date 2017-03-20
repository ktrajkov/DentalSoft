using DentalSoft.Data.Contracts.Mapping;
using DentalSoft.Data.Models.Patients;
namespace DentalSoft.Data.Contracts.Patientes
{
    public class PatientPlanModel : PresentationModel, IMapFrom<Patient>, IHaveCustomMappings
    {
        public string Phone { get; set; }

        public int DentistId { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Patient, PatientPlanModel>()
              .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.PersonalData.Contact.Telephone))
            .ForMember(p => p.DentistId, opt => opt.MapFrom(src => src.PersonalData.DentistId));
        }
    }
}
