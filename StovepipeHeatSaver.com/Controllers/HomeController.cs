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

        public async Task<ActionResult> Products()
        {
            return View(await db.Products.ToListAsync());
        }

        public async Task<ActionResult> Product(int? id)
        {
            Product product;
            string circumferenceParam = Request.Params.Get("circumference");
            string unit = Request.Params.Get("unit");

            // if id is specified, return that product
            if (id != null)
            {
                product = await db.Products.FindAsync(id);
            }

            // if circumference is specified, search for product that matches
            else if (circumferenceParam != null && unit != null)
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
                    var products = await db.Products.ToListAsync();
                    product = products.Single(p => circumference >= p.MinCircumference && circumference <= p.MaxCircumference);
                        
                }
                catch (Exception)
                {
                    return RedirectToAction("ProductSizeNotFound");
                }
            }
            else
            {
                return HttpNotFound();
            }
            return View(product);
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