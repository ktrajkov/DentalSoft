using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DentalSoft.Web.Startup))]
namespace DentalSoft.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
