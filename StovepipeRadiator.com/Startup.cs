using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StovepipeRadiator.Startup))]
namespace StovepipeRadiator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
