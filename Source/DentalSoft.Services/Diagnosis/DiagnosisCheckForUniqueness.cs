namespace DentalSoft.Services.Diagnosis
{
    using DentalSoft.Data.Services;
    using DentalSoft.Data.Models.Diagnoses;
    using DentalSoft.Data.Contracts.Diagnoses;
    using System.Linq;
    using DentalSoft.Data.Services.Exceptions;
    using DentalSoft.Data.Services.Interfaces;

    public class DiagnosisCheckForUniqueness : IEntityCheckForUniqueness<Diagnosis>
    {
        public void CheckForUniqueness(Diagnosis entity)
        {
            var persister = RepositoryManager.GetRepositoryForEntity<Diagnosis>();
            var filter = new DiagnosisFilter
            {
               Code = entity.Code
            };
            var result = persister.All<DiagnosisFilter>(filter);
            if (result.Any(x => x.Id != entity.Id))
            {
                throw new NonUniqueEntityException(Strings.DiagnosisCheckForUniqueness_CodeShouldBeUnique);
            }
        }
    }
}
