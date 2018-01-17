using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Cstieg.ControllerHelper;
using Cstieg.ControllerHelper.ActionFilters;
using Cstieg.Sales.Models;
using Cstieg.WebFiles.Controllers;
using Cstieg.WebFiles;
using StovepipeHeatSaver.Models;

namespace StovepipeHeatSaver.Controllers
{
    /// <summary>
    /// Controller to edit product model
    /// </summary>
    [ClearCache]
    [RoutePrefix("edit/products")]
    [Route("{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    // Allow HTML in ProductInfo field
    [ValidateInput(false)]
    public class ProductsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        [Route("")]
        public async Task<ActionResult> Index()
        {
            var products = await db.Products.Include(p => p.ShippingScheme).ToListAsync();
            foreach (var product in products)
            {
                product.WebImages = product.WebImages.OrderBy(w => w.Order).ToList();
            }
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public async Task<ActionResult> Create()
        {
            // delete images that were previously saved to newly created product that was not ultimately saved
            foreach (var webImage in await db.WebImages.Where(w => w.ProductId == null).ToListAsync())
            {
                // remove image files used by product
                imageManager.DeleteImageWithMultipleSizes(webImage.ImageUrl);

                db.WebImages.Remove(webImage);
                await db.SaveChangesAsync();
            }

            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Price,Shipping,ShippingSchemeId,ProductInfo,DisplayOnFrontPage,DoNotDisplay,Diameter")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();

                // connect images that were previously saved to product (id = null)
                foreach (var webImage in await db.WebImages.Where(w => w.ProductId == null).ToListAsync())
                {
                    webImage.ProductId = product.Id;
                    db.Entry(webImage).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", product.ShippingSchemeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Pass in list of images for product
            product.WebImages = product.WebImages ?? new List<WebImage>();
            product.WebImages = product.WebImages.OrderBy(w => w.Order).ToList();

            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", product.ShippingSchemeId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Price,Shipping,ShippingSchemeId,ProductInfo,DisplayOnFrontPage,DoNotDisplay,Diameter")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", product.ShippingSchemeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Adds an image to the product model
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Json result containing image id</returns>
        [HttpPost]
        public async Task<ActionResult> AddImage(int? id)
        {
            int? maxImageOrderNo = 0;

            // Allow null id for newly created product
            if (id != null)
            {
                Product product = await db.Products.FindAsync(id);
                if (product == null)
                {
                    return this.JError(404, "Can't find product " + id.ToString());
                }

                // Newly added image should go at end of collection unless purposefully reordered
                maxImageOrderNo = await db.WebImages.Where(w => w.ProductId == id).MaxAsync(w => w.Order) ?? 0;
            }

            // Check file is exists and is valid image
            HttpPostedFileBase imageFile = _ModelControllersHelper.GetImageFile(ModelState, Request, "", "file");

            // Save image to disk and store filepath in model
            try
            {
                string timeStamp = FileManager.GetTimeStamp();
                WebImage image = new WebImage
                {
                    ProductId = id,
                    ImageUrl = await imageManager.SaveFile(imageFile, 200, timeStamp),
                    ImageSrcSet = await imageManager.SaveImageMultipleSizes(imageFile, new List<int>() { 1600, 800, 400, 200 }, timeStamp),
                    Order = maxImageOrderNo + 1
                };
                db.WebImages.Add(image);
                await db.SaveChangesAsync();
                return PartialView("_ProductImagePartial", image);
            }
            catch (Exception e)
            {
                return this.JError(400, "Error saving image: " + e.Message);
            }
        }

        /// <summary>
        /// Deletes an image from the product model
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Json result containing image id</returns>
        [HttpPost]
        public async Task<JsonResult> DeleteImage(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return this.JError(404, "Can't find product " + id.ToString());
            }

            int imageId = int.Parse(Request.Params.Get("imageId"));
            WebImage image = await db.WebImages.FindAsync(imageId);
            if (image == null)
            {
                return this.JError(404, "Can't find image " + imageId.ToString());
            }

            // remove image files used by product
            imageManager.DeleteImageWithMultipleSizes(image.ImageUrl);

            db.WebImages.Remove(image);
            await db.SaveChangesAsync();
            return new JsonResult
            {
                Data = new
                {
                    success = "True",
                    imageId = image.Id
                }
            };
        }

        /// <summary>
        /// Updates the model from the Index table using EditIndex.js
        /// </summary>
        /// <param name="id">The Id of the model to update</param>
        /// <returns>A Json object indicating success status.  In case of error, returns object with data member containing the old product model,
        /// and the field causing the error if possible</returns>
        [HttpPost]
        public async Task<JsonResult> Update(int id)
        {
            Product existingProduct = await db.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return this.JError(404, "Can't find this product to update!");
            }

            try
            {
                Product newProduct = JsonConvert.DeserializeObject<Product>(Request.Params.Get("data"));
                newProduct.Id = id;

                db.Entry(existingProduct).State = EntityState.Detached;
                db.Entry(newProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (JsonReaderException e)
            {
                var returnData = JsonConvert.SerializeObject(new { product = existingProduct, error = e.Message, field = e.Path });
                return this.JError(400, "Invalid data!", returnData);
            }
            catch (Exception e)
            {
                var returnData = JsonConvert.SerializeObject(new { product = existingProduct, error = e.Message });
                return this.JError(400, "Unable to save!", returnData);
            }
            return this.JOk();
        }

        /// <summary>
        /// Saves an image sort to database by numbering the Order field
        /// </summary>
        /// <param name="id">Id of the product whose images to sort</param>
        /// <returns>JSON object indicating success</returns>
        [HttpPost]
        public async Task<JsonResult> OrderWebImages(int? id)
        {
            List<WebImage> webImages = await db.WebImages.Where(w => w.ProductId == id).ToListAsync();

            List<string> imageOrder = JsonConvert.DeserializeObject<List<string>>(Request.Params.Get("imageOrder"));
            for (int i = 0; i < imageOrder.Count(); i++)
            {
                string imageId = imageOrder[i];
                WebImage webImage = await db.WebImages.FindAsync(int.Parse(imageId));
                webImage.Order = i;
                db.Entry(webImage).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return this.JOk();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
