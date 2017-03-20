namespace DentalSoft.Services.Treatments
{
    using DentalSoft.Data.Contracts.Treatments;
    using DentalSoft.Data.Models.Treatments;
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Services.Exceptions;
    using DentalSoft.Data.Services.Interfaces;
    using System.Linq;

    public class TreatmentCheckForUniqueness : IEntityCheckForUniqueness<Treatment>
    {
        public void CheckForUniqueness(Treatment entity)
        {
            var persister = RepositoryManager.GetRepositoryForEntity<Treatment>();
            var filter = new TreatmentFilter
            {
                DiagnosisId = entity.DiagnosisId,
                Description = entity.Description
            };
            var result = persister.All<TreatmentFilter>(filter);
            if (result.Any(x => x.Id != entity.Id))
            {
                throw new NonUniqueEntityException(Strings.TreatmentCheckForUniqueness_DescriptionShouldBeUnique);
            }
        }
    }
}
