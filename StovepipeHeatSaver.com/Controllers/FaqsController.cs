using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Cstieg.ControllerHelper.ActionFilters;
using StovepipeHeatSaver.Models;

namespace StovepipeHeatSaver.Controllers
{
    [ClearCache]
    [RoutePrefix("edit/faqs")]
    [Route("{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    [ValidateInput(false)]
    public class FaqsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Faqs
        [Route("")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Faqs.ToListAsync());
        }

        // GET: Faqs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = await db.Faqs.FindAsync(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // GET: Faqs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faqs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Question,Answer")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                db.Faqs.Add(faq);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(faq);
        }

        // GET: Faqs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Faq faq = await db.Faqs.FindAsync(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Question,Answer")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                db.Entry(faq).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(faq);
        }

        // GET: Faqs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Faq faq = await db.Faqs.FindAsync(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Faq faq = await db.Faqs.FindAsync(id);
            db.Faqs.Remove(faq);
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
