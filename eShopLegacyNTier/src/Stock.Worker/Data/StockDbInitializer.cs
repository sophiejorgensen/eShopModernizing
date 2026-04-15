using System;
using System.Collections.Generic;
using System.Data.Entity;
using eShop.Contracts.Models;

namespace Stock.Worker.Data
{
    public class StockDbInitializer : CreateDatabaseIfNotExists<StockDbContext>
    {
        protected override void Seed(StockDbContext context)
        {
            var today = DateTime.UtcNow.Date;
            context.CatalogItemStocks.AddRange(new List<CatalogItemStock>
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
            context.SaveChanges();
        }
    }
}
