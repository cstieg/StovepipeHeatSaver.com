using StovepipeHeatSaver.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StovepipeHeatSaver.Controllers
{
    //[OutputCache(CacheProfile = "CacheForADay")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Product()
        {
            string circumferenceParam = Request.Params.Get("circumference");
            string unit = Request.Params.Get("unit");
            if (circumferenceParam != null && unit != null)
            {
                decimal circumference = decimal.Parse(circumferenceParam);
                switch (unit)
                {
                    case "inches":
                        break;
                    case "centimeters":
                        circumference /= 2.54M;
                        break;
                    default:
                        throw new Exception("Invalid unit specification");
                }
                
                try
                {
                    Product product = await db.Products
                        .Where(p => circumference >= p.MinCircumference && circumference <= p.MaxCircumference)
                        .SingleAsync();
                    return View(product);
                }
                catch (Exception)
                {
                    return RedirectToAction("ProductSizeNotFound");
                }
            }

            return View();
        }

        public ActionResult ProductSizeNotFound()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> Faq()
        {
            return View(await db.Faqs.ToListAsync());
        }

        public async Task<ActionResult> Reviews()
        {
            return View(await db.Reviews.ToListAsync());
        }

        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Displays list of links to model edit pages
        /// </summary>
        public ActionResult Edit()
        {
            string modelControllers = ConfigurationManager.AppSettings["modelControllers"];
            char[] delimiters = { ',' };
            string[] controllersArray = modelControllers.Split(delimiters);
            List<string> controllers = new List<string>(controllersArray);
            return View(controllers);
        }
    }
}