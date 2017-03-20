namespace DentalSoft.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class AssemblyHelpers
    {
       
        public static string GetDirectoryForAssembyl(Assembly assembly)
        {
            var assemblyLocation = assembly.CodeBase;
            var location = new UriBuilder(assemblyLocation);
            var path = Uri.UnescapeDataString(location.Path);
            var directory = Path.GetDirectoryName(path);
            return directory;
        }


        private static Dictionary<string, Type> typeCache=new Dictionary<string,Type>();

        public static bool TryFindType(string typeName, out Type t) {
            lock (typeCache) {
                if (!typeCache.TryGetValue(typeName, out t)) {
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                        t = a.GetType(typeName);
                        if (t != null)
                            break;
                    }
                    typeCache[typeName] = t; // perhaps null
                }
            }
            return t != null;
        }
    }
}
