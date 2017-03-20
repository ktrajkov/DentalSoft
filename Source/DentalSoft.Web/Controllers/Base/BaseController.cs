namespace DentalSoft.Web.Controllers.Base
{
    using DentalSoft.Common;
    using DentalSoft.Web.Infrastructure.ActionResults.Base;
    using DentalSoft.Web.Infrastructure.Extensions;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class BaseController : Controller
    {

        [NonAction]
        protected JsonNetResult JsonNet(object data)
        {
            return this.JsonNet(data, null, null, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        protected internal virtual JsonNetResult JsonNet(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            ExceptionUtil.NotNull(data, "data");
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new System.ArgumentNullException("filterContext");
            }

            if (!filterContext.IsChildAction && !filterContext.ExceptionHandled && this.Request.IsAjaxRequest())
            {
                if (this.Request.IsAjaxRequest())
                {
                    var exception = filterContext.Exception;
                    if (new HttpException(null, exception).GetHttpCode() == 500)
                    {
                        string controllerName = filterContext.HttpContext.GetControllerName();
                        string actionName = filterContext.HttpContext.GetActionName();

                        filterContext.Result = new JsonNetResult
                        {
                            Data = new
                            {
                                Area = filterContext.HttpContext.GetAreaName(),
                                Action = actionName,
                                Controller = controllerName,
                                Message = exception.Message,
                                Exception = exception.ToString()
                            },
                            JsonRequestBehavior = 0
                        };
                        filterContext.ExceptionHandled = true;
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.StatusCode = 500;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    }
                }
                else
                {
                    var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                    var actionName = ControllerContext.RouteData.Values["action"].ToString();
                    this.View("Errors", new HandleErrorInfo(filterContext.Exception, controllerName, actionName)).ExecuteResult(this.ControllerContext);
                }
            }
        }
    }
}