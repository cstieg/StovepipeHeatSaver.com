using System.Web.Mvc;
using StovepipeRadiator.Models;

namespace StovepipeRadiator.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
    }
}