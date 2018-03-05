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

            // Add search folder to Razor ViewEngine
            ViewEngines.Engines.Clear();
            var razorEngine = new RazorViewEngine();
            razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats
                .Concat(new[]
                {
                    "~/Views/ModelViews/{1}/{0}.cshtml"
                }).ToArray();
            razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats
                .Concat(new[]
                {
                    "~/Views/ModelViews/{1}/{0}.cshtml",
                    "~/Views/ModelViews/{0}.cshtml"
                }).ToArray();
            ViewEngines.Engines.Add(razorEngine);
        }
    }
}
