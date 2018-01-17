﻿using Cstieg.ControllerHelper;
using Cstieg.Sales.Models;
using RazorEngine;
using RazorEngine.Templating;
using StovepipeHeatSaver.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StovepipeHeatSaver.Controllers
{
    public class MailController : BaseController
    {
        // POST: Mail/ConfirmOrder?cart=DF39FEI314040
        /// <summary>
        /// Sends confirmation email for completed order.
        /// </summary>
        /// <param name="cart">Alphanumeric cart id assigned to order by PayPal</param>
        /// <returns>JSON success=true result </returns>
        [HttpPost]
        public async Task<ActionResult> ConfirmOrder()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string id = Request.Params.Get("cart");
            Order order = await db.Orders.Include(o => o.Customer).Where(o => o.Cart == id).SingleOrDefaultAsync();
            //Order order = await db.Orders.Include(o => o.Customer).FirstOrDefaultAsync();
            if (order == null)
            {
                return HttpNotFound();
            }
            order.ShipToAddress = await db.Addresses.FindAsync(order.ShipToAddressId);

            string templatePath = Server.MapPath("~/Views/Mail/OrderSuccessEmail.cshtml");
            var sr = new StreamReader(templatePath);
            string body = Engine.Razor.RunCompile(await sr.ReadToEndAsync(), "orderSuccessEmail", null, order);

            var message = new MailMessage
            {
                From = new MailAddress("webmaster@stovepipeheatsaver.com"),
                Subject = "Order confirmation - " + order.Description,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(order.Customer.EmailAddress));

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
            }
            return this.JOk();
        }
    }
}