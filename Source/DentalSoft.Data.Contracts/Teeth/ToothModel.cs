using DentalSoft.Data.Contracts.Mapping;
using DentalSoft.Data.Models.Teeths;
using System;
namespace DentalSoft.Data.Contracts.Teeth
{
    public class ToothModel : PresentationModel, IMapFrom<Tooth>
    {
        public int Number { get; set; }
    }
}
