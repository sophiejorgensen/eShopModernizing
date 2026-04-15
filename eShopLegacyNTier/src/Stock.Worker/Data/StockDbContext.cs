using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eShop.Contracts.Models;

namespace Stock.Worker.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext() : base("name=StockDb") { }

        public DbSet<CatalogItemStock> CatalogItemStocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogItemStock>()
                .HasKey(e => e.StockId)
                .ToTable("CatalogItemsStock");
        }

        public static void EnsureSeedData()
        {
            using (var db = new StockDbContext())
            {
                // Create table if it doesn't exist
                db.Database.ExecuteSqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CatalogItemsStock')
                    BEGIN
                        CREATE TABLE CatalogItemsStock (
                            StockId INT IDENTITY(1,1) PRIMARY KEY,
                            CatalogItemId INT NOT NULL,
                            Date DATETIME NOT NULL,
                            AvailableStock INT NOT NULL
                        )
                    END");

                if (!db.CatalogItemStocks.Any())
                {
                    var today = DateTime.UtcNow.Date;
                    db.CatalogItemStocks.AddRange(new List<CatalogItemStock>
                    {
                        new CatalogItemStock { CatalogItemId = 1, Date = today, AvailableStock = 85 },
                        new CatalogItemStock { CatalogItemId = 2, Date = today, AvailableStock = 120 },
                        new CatalogItemStock { CatalogItemId = 3, Date = today, AvailableStock = 45 },
                        new CatalogItemStock { CatalogItemId = 4, Date = today, AvailableStock = 200 },
                        new CatalogItemStock { CatalogItemId = 5, Date = today, AvailableStock = 8 },
                        new CatalogItemStock { CatalogItemId = 6, Date = today, AvailableStock = 150 },
                        new CatalogItemStock { CatalogItemId = 7, Date = today, AvailableStock = 30 },
                        new CatalogItemStock { CatalogItemId = 8, Date = today, AvailableStock = 5 },
                        new CatalogItemStock { CatalogItemId = 9, Date = today, AvailableStock = 75 },
                        new CatalogItemStock { CatalogItemId = 10, Date = today, AvailableStock = 60 },
                        new CatalogItemStock { CatalogItemId = 11, Date = today, AvailableStock = 90 },
                        new CatalogItemStock { CatalogItemId = 12, Date = today, AvailableStock = 110 }
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}
