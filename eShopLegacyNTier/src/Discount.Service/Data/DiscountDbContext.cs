using System.Data.Entity;
using eShop.Contracts.Models;

namespace Discount.Service.Data
{
    public class DiscountDbContext : DbContext
    {
        public DiscountDbContext() : base("name=DiscountDb") { }

        public DbSet<DiscountItem> DiscountItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscountItem>().ToTable("DiscountItems");
        }
    }
}
