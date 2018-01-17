using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cstieg.ControllerHelper.ActionFilters;
using StovepipeHeatSaver.Models;

namespace StovepipeHeatSaver.Controllers
{
    /// <summary>
    /// Controller to edit reviews model
    /// </summary>
    [ClearCache]
    [RoutePrefix("edit/reviews")]
    [Route("{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    // Allow HTML in testimonial text
    [ValidateInput(false)]
    public class ReviewsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        [Route("")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Reviews.OrderByDescending(r => r.Date).ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Review review = await db.Reviews.FindAsync(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Person,Date,Location,Text")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Review review = await db.Reviews.FindAsync(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Person,Date,Location,Text")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Review review = await db.Reviews.FindAsync(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Review review = await db.Reviews.FindAsync(id);
            db.Reviews.Remove(review);
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
