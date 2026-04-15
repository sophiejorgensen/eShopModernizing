using System.Collections.Generic;
using System.Data.Entity;
using eShop.Contracts.Models;

namespace Catalog.Service.Data
{
    public class CatalogDbInitializer : CreateDatabaseIfNotExists<CatalogDbContext>
    {
        protected override void Seed(CatalogDbContext context)
        {
            context.CatalogBrands.AddRange(new List<CatalogBrand>
            {
                new CatalogBrand { Id = 1, Brand = "Azure" },
                new CatalogBrand { Id = 2, Brand = ".NET" },
                new CatalogBrand { Id = 3, Brand = "Visual Studio" },
                new CatalogBrand { Id = 4, Brand = "SQL Server" },
                new CatalogBrand { Id = 5, Brand = "Other" }
            });

            context.CatalogTypes.AddRange(new List<CatalogType>
            {
                new CatalogType { Id = 1, Type = "Mug" },
                new CatalogType { Id = 2, Type = "T-Shirt" },
                new CatalogType { Id = 3, Type = "Sheet" },
                new CatalogType { Id = 4, Type = "USB Memory Stick" }
            });

            context.CatalogItems.AddRange(new List<CatalogItem>
            {
                new CatalogItem { Id = 1, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5m, PictureFileName = "2.png" },
                new CatalogItem { Id = 2, CatalogTypeId = 1, CatalogBrandId = 2, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price = 8.50m, PictureFileName = "11.png" },
                new CatalogItem { Id = 3, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12m, PictureFileName = "7.png" },
                new CatalogItem { Id = 4, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12m, PictureFileName = "5.png" },
                new CatalogItem { Id = 5, CatalogTypeId = 3, CatalogBrandId = 5, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5m, PictureFileName = "9.png" },
                new CatalogItem { Id = 6, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12m, PictureFileName = "1.png" },
                new CatalogItem { Id = 7, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12m, PictureFileName = "6.png" },
                new CatalogItem { Id = 8, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5m, PictureFileName = "3.png" },
                new CatalogItem { Id = 9, CatalogTypeId = 1, CatalogBrandId = 5, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12m, PictureFileName = "12.png" },
                new CatalogItem { Id = 10, CatalogTypeId = 3, CatalogBrandId = 2, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12m, PictureFileName = "8.png" },
                new CatalogItem { Id = 11, CatalogTypeId = 3, CatalogBrandId = 2, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5m, PictureFileName = "10.png" },
                new CatalogItem { Id = 12, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Cup<T> TShirt", Name = "Cup<T> TShirt", Price = 12m, PictureFileName = "4.png" }
            });

            context.SaveChanges();
        }
    }
}
