using Cstieg.Sales.Models;
using Cstieg.Sales.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StovepipeHeatSaver.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, ISalesDbContext, IProductExtensionContext
    {
        public ApplicationDbContext()
            : base(ConfigurationManager.AppSettings["DbConnection"], throwIfV1Schema: false)
        {
        }

        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Store> Stores { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<PromoCodeAdded> PromoCodesAdded { get; set; }
        public DbSet<ShippingCountry> ShippingCountries { get; set; }
        public DbSet<ShippingScheme> ShippingSchemes { get; set; }
        public DbSet<WebImage> WebImages { get; set; }

        public DbSet<ProductExtension> ProductExtensions { get; set; }

        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}