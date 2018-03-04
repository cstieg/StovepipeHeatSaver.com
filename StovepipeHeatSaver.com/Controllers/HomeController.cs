using StovepipeHeatSaver.Models;
using StovepipeHeatSaver.Services;
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
    [OutputCache(CacheProfile = "CacheForAWeek")]
    public class HomeController : BaseController
    {
        // GET: /
        public ActionResult Index()
        {
            return View();
        }

        // GET: Products
        public async Task<ActionResult> Products()
        {
            var products = await _productService.GetDisplayProductsAsync();
            return View(products);
        }

        // GET: Product?circumference=10.8&unit=inches
        // GET: Product/1
        // GET: Product/StovepipeHeatSaver for 6" stove pipe
        /// <summary>
        /// Routes product page request to the appropriate controller method
        /// </summary>
        /// <param name="id">May be an integer id or string product name</param>
        public async Task<ActionResult> Product(string id)
        {
            if (id == null || id == "")
            {
                return await ProductByCircumference(Request);
            }

            try
            {
                return await ProductById(int.Parse(id));
            }
            catch
            {
                return await ProductByProductName(id);
            }
        }


        // GET: Product?circumference=10.8&unit=inches
        public async Task<ActionResult> ProductByCircumference(HttpRequestBase Request)
        {
            string circumferenceParam = Request.Params.Get("circumference");
            string unit = Request.Params.Get("unit");
            decimal circumference;

            if (circumferenceParam == null || unit == null)
            {
                return HttpNotFound();
            }

            try
            {
                circumference = decimal.Parse(circumferenceParam);
            }
            catch (Exception)
            {
                return HttpNotFound("Please enter valid decimal.");
            }

            switch (unit)
            {
                case "inches":
                    break;
                case "centimeters":
                    circumference /= 2.54M;
                    break;
                default:
                    return HttpNotFound("Invalid Unit Specification");
            }

            try
            {
                var products = await _productService.GetProductsForCircumferenceAsync(circumference);
                switch (products.Count)
                {
                    case 0:
                        throw new Exception("Product size not found");
                    case 1:
                        var product = products.Single();
                        return View(product);
                    default:
                        return View("Products", products);
                }       
            }
            catch (Exception)
            {
                string circumferenceString = circumference.ToString() + "%20" + unit;
                ViewBag.MailToString = "mailto:stieg_d@yahoo.com?subject=Custom%20size%20Stovepipe%20HeatSaver%20-%20" + circumferenceString +
                                        "&body=Please%20contact%20me%20with%20information%20about%20a%20custom%20size%20HeatSaver.%20" +
                                        "I%20have%20measured%20the%20circumference%20of%20my%20stovepipe%20at%20" + circumferenceString + ".";
                return View("ProductSizeNotFound");
            }
        }

        // GET: Product/1
        public async Task<ActionResult> ProductById(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View("Product", product);
        }

        // GET: Product/StovepipeHeatSaver for 6" stove pipe
        public async Task<ActionResult> ProductByProductName(string productName)
        {
            var product = await _productService.GetByNameAsync(productName);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View("Product", product);
        }

        // GET: ProductSizeNotFound
        public ActionResult ProductSizeNotFound()
        {
            return View();
        }
        
        // GET: Faq
        public async Task<ActionResult> Faq()
        {
            return View(await _context.Faqs.ToListAsync());
        }

        // GET: Reviews
        public async Task<ActionResult> Reviews()
        {
            return View(await _context.Reviews.OrderByDescending(r => r.Date).ToListAsync());
        }

        // GET: Contact
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