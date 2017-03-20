namespace DentalSoft.Common
{
    using DentalSoft.Common.Constants;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class RegionsGenerator
    {
        public static IEnumerable<dynamic> Regions()
        {
            //TODO: Create function to load files
            var directory = AssemblyHelpers.GetDirectoryForAssembyl(Assembly.GetExecutingAssembly());
            var lines = File.ReadAllLines(directory + Files.RegionSource, Encoding.Default);

            IEnumerable<dynamic> allMunicipalities = GetMunicipalities();

            var regions = new List<dynamic>();
            foreach (string line in lines)
            {
                dynamic regionFromPhpArray = ParsePhpArray(line);

                var municipalitiesForRegion = allMunicipalities.Where(a => regionFromPhpArray.Items.Contains(a.Name)).ToList();
                regions.Add(new { Name = regionFromPhpArray.Key, Municipalitys = municipalitiesForRegion });
            }
            return regions;
        }

        #region Private Members
        private static IEnumerable<dynamic> GetMunicipalities()
        {
            var directory = AssemblyHelpers.GetDirectoryForAssembyl(Assembly.GetExecutingAssembly());
            var lines = File.ReadAllLines(directory + Files.MunicipalitySource, Encoding.Default);
            var municipalities = new List<dynamic>();

            foreach (string line in lines)
            {
                dynamic municipalityFromPhpArray = ParsePhpArray(line);

                var locations = ((IEnumerable<string>)(municipalityFromPhpArray.Items)).Select(s => new { Name = s }).ToList();
                municipalities.Add(new { Name = municipalityFromPhpArray.Key, Locations = locations });

            }

            return municipalities;
        }

        private static dynamic ParsePhpArray(string line)
        {
            //$arr['Банско'] = Array('Гостун','Добринище','Кремен','Места','Обидим','Осеново','Филипово');

            var keyStartIndex = 6;
            var keyEndIndex = line.IndexOf("']");
            var key = line.Substring(keyStartIndex, keyEndIndex - keyStartIndex);

            var itemsStartIndex = line.IndexOf('(') + 1;
            var itemsEndIndex = line.IndexOf(')');

            var itemsString = line.Substring(itemsStartIndex, itemsEndIndex - itemsStartIndex);
            var items = itemsString.Split(',').Select(s => s.Trim(new char[] { '\'', ' ' })).ToList();
            return new
            {
                Key = key,
                Items = items
            };
        }
        #endregion
    }
}
