﻿using Microsoft.AspNet.Identity;
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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(ConfigurationManager.AppSettings["DbConnection"], throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Cstieg.Sales.Models.ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Cstieg.Sales.Models.Customer> Customers { get; set; }
        public DbSet<Cstieg.Sales.Models.Order> Orders { get; set; }
        public DbSet<Cstieg.Sales.Models.OrderDetail> OrderDetails { get; set; }
        public DbSet<Cstieg.Sales.Models.ShipToAddress> Addresses { get; set; }
        public DbSet<Cstieg.Sales.Models.ShippingScheme> ShippingSchemes { get; set; }
        public DbSet<Cstieg.Sales.Models.ShippingCountry> ShippingCountries { get; set; }
        public DbSet<Cstieg.Sales.Models.WebImage> WebImages { get; set; }
        public DbSet<Cstieg.Geography.Country> Countries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}