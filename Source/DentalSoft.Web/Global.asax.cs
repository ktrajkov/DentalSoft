namespace DentalSoft.Web
{
    using DentalSoft.Data.Contracts.Mapping;
    using DentalSoft.Data.Services;
    using DentalSoft.Web.Controllers.Base;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Linq;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute();

            RepositoryManager.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine()); 
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            RepositoryManager.Dispose();
        }

      
        
    }
}
