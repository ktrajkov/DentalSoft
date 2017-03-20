using DentalSoft.Common;
using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;

namespace DentalSoft.Web.Infrastructure.ActionResults.Base
{
    public class JsonNetResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        public System.Text.Encoding ContentEncoding
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Specifies Newtonsoft.Json.JsonSerializer settings.
        /// </summary>
        public JsonSerializerSettings SerializerSettings
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the Newtonsoft.Json.JsonTextWriter formatting.
        /// </summary>
        public Formatting Formatting
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the JsonNetResult class.
        /// </summary>
        public JsonNetResult()
        {
            this.SerializerSettings = new JsonSerializerSettings();
            this.JsonRequestBehavior =JsonRequestBehavior.AllowGet;
        }
        /// <summary>
        /// Enables processing of the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes
        /// the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            ExceptionUtil.NotNull(context, "context");
            string httpMethod = context.HttpContext.Request.HttpMethod;
            if (this.JsonRequestBehavior == JsonRequestBehavior.AllowGet && string.Equals(httpMethod, "GET", System.StringComparison.OrdinalIgnoreCase))
            {
                throw new System.InvalidOperationException("You can't access this action with GET");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = ((!string.IsNullOrEmpty(this.ContentType)) ? this.ContentType : "application/json");
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                JsonTextWriter jsonTextWriter = new JsonTextWriter(response.Output);
                jsonTextWriter.Formatting = this.Formatting;
                JsonTextWriter jsonTextWriter2 = jsonTextWriter;
                JsonSerializer jsonSerializer = JsonSerializer.Create(this.SerializerSettings);
                jsonSerializer.Serialize(jsonTextWriter2, this.Data);
                jsonTextWriter2.Flush();
            }
        }
    }
}
