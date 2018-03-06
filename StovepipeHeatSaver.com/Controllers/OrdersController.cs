using Cstieg.Sales.Models;
using StovepipeHeatSaver.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StovepipeHeatSaver.Controllers
{
    /// <summary>
    /// The controller providing model scaffolding for Orders
    /// </summary>
    [RoutePrefix("edit/orders")]
    [Route("{action}/{id?}")]
    [Authorize(Roles = "Administrator")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Route("")]
        public async Task<ActionResult> Index()
        {
            var orders = await db.Orders.Include(o => o.Customer).Include(o => o.OrderDetails)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Where(o => o.CustomerId != null).OrderByDescending(o => o.DateOrdered)
                .Take(100).ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Order order = await db.Orders.Include(o => o.OrderDetails.Select(od => od.Product)).SingleOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Order order = await db.Orders.Include(o => o.OrderDetails.Select(od => od.Product)).SingleOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
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
