using DentalSoft.Data.Contracts.Mapping;
using DentalSoft.Data.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalSoft.Data.Contracts.Deseases
{
    public class DeseaseCategoryModel : PresentationModel, IMapFrom<DeseaseCategory>
    {
        public string  Name { get; set; }

        public ICollection<DeseaseModel> Deseases { get; set; }
    }
}
