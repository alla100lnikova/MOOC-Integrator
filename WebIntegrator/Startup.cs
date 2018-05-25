using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebIntegrator.Startup))]
namespace WebIntegrator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
