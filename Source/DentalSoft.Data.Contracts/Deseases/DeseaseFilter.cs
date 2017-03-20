using DentalSoft.Common.Mapping;
namespace DentalSoft.Data.Contracts.Deseases
{
    public class DeseaseFilter : EntityFilter
    {
        [MapAssociation("DeseaseCategory.Status.Name")]
        public string StatusName { get; set; }
    }
}
