namespace DentalSoft.Web.Infrastructure.Extensions
{
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// A class for RequestContext extenssion methods.
    /// </summary>
    public static class RequestContextExtensions
    {
        /// <summary>
        /// Gets the name of the area from the current request.s
        /// </summary>
        /// <param name="context">The HttpContextBase object.</param>
        /// <returns>The area name.</returns>
        public static string GetAreaName(this HttpContextBase context)
        {
            RouteValueDictionary dataTokens = context.Request.RequestContext.RouteData.DataTokens;
            if (!dataTokens.ContainsKey("area"))
            {
                return string.Empty;
            }
            return dataTokens["area"].ToString();
        }
        /// <summary>
        /// Gets the name of the controller from the current request.s
        /// </summary>
        /// <param name="context">The HttpContextBase object.</param>
        /// <returns>The controller name.</returns>
        public static string GetControllerName(this HttpContextBase context)
        {
            return context.Request.RequestContext.RouteData.GetRequiredString("controller");
        }
        /// <summary>
        /// Gets the name of the action from the current request.s
        /// </summary>
        /// <param name="context">The HttpContextBase object.</param>
        /// <returns>The action name.</returns>
        public static string GetActionName(this HttpContextBase context)
        {
            return context.Request.RequestContext.RouteData.GetRequiredString("action");
        }
    }
}
