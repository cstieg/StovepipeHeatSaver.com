using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Cstieg.Sales.Models;

namespace StovepipeHeatSaver.Controllers
{
    public class ShippingCountriesController : BaseController
    {

        // GET: ShippingCountries
        public async Task<ActionResult> Index()
        {
            var shippingCountries = db.ShippingCountries.Include(s => s.Country).Include(s => s.ShippingScheme);
            return View(await shippingCountries.ToListAsync());
        }

        // GET: ShippingCountries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCountry shippingCountry = await db.ShippingCountries.FindAsync(id);
            if (shippingCountry == null)
            {
                return HttpNotFound();
            }
            return View(shippingCountry);
        }

        // GET: ShippingCountries/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");
            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name");
            return View();
        }

        // POST: ShippingCountries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ShippingSchemeId,CountryId,MinQty,MaxQty,AdditionalShipping")] ShippingCountry shippingCountry)
        {
            if (ModelState.IsValid)
            {
                db.ShippingCountries.Add(shippingCountry);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", shippingCountry.CountryId);
            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", shippingCountry.ShippingSchemeId);
            return View(shippingCountry);
        }

        // GET: ShippingCountries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCountry shippingCountry = await db.ShippingCountries.FindAsync(id);
            if (shippingCountry == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", shippingCountry.CountryId);
            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", shippingCountry.ShippingSchemeId);
            return View(shippingCountry);
        }

        // POST: ShippingCountries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ShippingSchemeId,CountryId,MinQty,MaxQty,AdditionalShipping")] ShippingCountry shippingCountry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingCountry).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", shippingCountry.CountryId);
            ViewBag.ShippingSchemeId = new SelectList(db.ShippingSchemes, "Id", "Name", shippingCountry.ShippingSchemeId);
            return View(shippingCountry);
        }

        // GET: ShippingCountries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCountry shippingCountry = await db.ShippingCountries.FindAsync(id);
            if (shippingCountry == null)
            {
                return HttpNotFound();
            }
            return View(shippingCountry);
        }

        // POST: ShippingCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ShippingCountry shippingCountry = await db.ShippingCountries.FindAsync(id);
            db.ShippingCountries.Remove(shippingCountry);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
