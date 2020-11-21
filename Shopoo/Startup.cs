using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopoo.Startup))]
namespace Shopoo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
