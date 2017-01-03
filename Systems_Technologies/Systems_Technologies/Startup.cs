using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Systems_Technologies.Startup))]
namespace Systems_Technologies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
