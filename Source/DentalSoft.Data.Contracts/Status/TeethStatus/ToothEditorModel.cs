namespace DentalSoft.Data.Contracts.Status.TeethStatus
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Models.Operation;
    using DentalSoft.Data.Models.Status.TeethStatus;
    using DentalSoft.Common.Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class ToothEditorModel : PresentationModel, IMapFrom<ToothChart>
    {
        public ChartType? Type { get; set; }

        public ColorType? Color { get; set; }

        public string ChartTypeText
        {
            get { return Type.GetDescription(); }
        }

        public string ColorTypeText
        {
            get { return Color.GetDescription(); }
        }
    }
}
