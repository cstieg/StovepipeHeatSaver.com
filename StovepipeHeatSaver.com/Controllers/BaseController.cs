using System.Web.Mvc;
using StovepipeHeatSaver.Models;
using Cstieg.WebFiles;

namespace StovepipeHeatSaver.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        public static string contentFolder = "/content";

        protected IFileService storageService;
        protected ImageManager imageManager;

        public BaseController()
        {
            // Set storage service for product images
            storageService = new FileSystemService(contentFolder);
            imageManager = new ImageManager("images/products", storageService);
        }

        /// <summary>
        /// Add dependency to cache so it is refreshed when updating the dependency
        /// </summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.AddCacheItemDependency("Pages");
        }
    }
}