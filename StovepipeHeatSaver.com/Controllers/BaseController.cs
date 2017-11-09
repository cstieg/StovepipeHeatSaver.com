using System.Web.Mvc;
using StovepipeHeatSaver.Models;

namespace StovepipeHeatSaver.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
    }
}