namespace DentalSoft.Data.Models.Status.TeethStatus
{
    using DentalSoft.Data.Models.Operation;

    public class ToothChart : DeletableEntity
    {
        public ChartType? Type { get; set; }

        public ColorType? Color { get; set; }
    }
}
