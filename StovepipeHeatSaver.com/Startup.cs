using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StovepipeHeatSaver.Startup))]
namespace StovepipeHeatSaver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
