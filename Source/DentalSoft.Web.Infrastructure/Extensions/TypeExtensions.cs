namespace DentalSoft.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages.Html;
    using DentalSoft.Common.Extensions;

    public static class TypeExtensions
    {
        public static List<SelectListItem> EnumToSelectList(this Type enumType)
        {
            if (enumType.IsEnum)
            {
                var selectListItems = from Enum item in Enum.GetValues(enumType)
                    select new SelectListItem
                    {
                        Value = Convert.ToInt32(item).ToString(),
                        Text = item.GetDescription(),
                    };
                return selectListItems.ToList();
            }
            else
            {
                throw new ArgumentException("The Type should be of type Enum");
            }
        }
    }
}
