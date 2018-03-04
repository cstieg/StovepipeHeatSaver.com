using Cstieg.Sales.Repositories;
using System.Data.Entity;

namespace StovepipeHeatSaver.Models
{
    public interface IProductExtensionContext : ISalesDbContext
    {
        DbSet<ProductExtension> ProductExtensions { get; set; }
    }
}
