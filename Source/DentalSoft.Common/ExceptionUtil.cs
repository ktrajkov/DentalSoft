namespace DentalSoft.Common
{
    using System;

    public static class ExceptionUtil
    {
        /// <summary>
        /// Checks if a parameter is not null.
        /// </summary>
        /// <param name="param">The parameter to be checked.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void NotNull(object param, string paramName)
        {
            if (paramName == null)
            {
                throw new ArgumentNullException("paramName", "paramName not set.");
            }
            if (paramName.Length == 0)
            {
                throw new ArgumentException("paramName cannot be empty.", "paramName");
            }
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
        /// <summary>
        /// Checks if a parameter is not empty. (of type string)
        /// </summary>
        /// <param name="param">The parameter to be checked.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void NotEmpty(string param, string paramName)
        {
            ExceptionUtil.NotNull(param, paramName);
            if (param.Length == 0)
            {
                throw new ArgumentException(paramName + " cannot be empty.", paramName);
            }
        }
    }
}
