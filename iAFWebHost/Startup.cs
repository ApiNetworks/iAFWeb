using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iAFWebHost.Startup))]
namespace iAFWebHost
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
