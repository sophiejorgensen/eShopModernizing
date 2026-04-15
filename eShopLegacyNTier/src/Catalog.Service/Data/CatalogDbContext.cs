using System.Data.Entity;
using eShop.Contracts.Models;

namespace Catalog.Service.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext() : base("name=CatalogDb")
        {
            Database.SetInitializer(new CatalogDbInitializer());
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogBrand>()
                .Property(e => e.Brand).HasMaxLength(50).IsUnicode(false);

            modelBuilder.Entity<CatalogItem>()
                .Property(e => e.Price).HasPrecision(19, 4);

            modelBuilder.Entity<CatalogType>()
                .Property(e => e.Type).HasMaxLength(50).IsUnicode(false);
        }
    }
}
