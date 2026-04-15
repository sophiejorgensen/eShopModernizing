using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Catalog.Service.Data;
using eShop.Contracts.Events;
using eShop.Contracts.Models;
using eShop.EventBus;

namespace Catalog.Service.Controllers
{
    [RoutePrefix("api/catalog")]
    public class CatalogController : ApiController
    {
        // In .NET Framework: static singleton event bus.
        // After ACA migration: injected Dapr client via DI.
        private static readonly IEventBus EventBus = EventBusInstance.Bus;

        [Route("items"), HttpGet]
        public IHttpActionResult GetItems(int brandId = 0, int typeId = 0)
        {
            using (var db = new CatalogDbContext())
            {
                var query = db.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .AsQueryable();

                if (brandId > 0)
                    query = query.Where(c => c.CatalogBrandId == brandId);
                if (typeId > 0)
                    query = query.Where(c => c.CatalogTypeId == typeId);

                return Ok(query.ToList());
            }
        }

        [Route("items/{id:int}"), HttpGet]
        public IHttpActionResult GetItem(int id)
        {
            using (var db = new CatalogDbContext())
            {
                var item = db.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .FirstOrDefault(c => c.Id == id);

                if (item == null) return NotFound();
                return Ok(item);
            }
        }

        [Route("brands"), HttpGet]
        public IHttpActionResult GetBrands()
        {
            using (var db = new CatalogDbContext())
            {
                return Ok(db.CatalogBrands.ToList());
            }
        }

        [Route("types"), HttpGet]
        public IHttpActionResult GetTypes()
        {
            using (var db = new CatalogDbContext())
            {
                return Ok(db.CatalogTypes.ToList());
            }
        }

        [Route("items"), HttpPost]
        public IHttpActionResult CreateItem(CatalogItem item)
        {
            using (var db = new CatalogDbContext())
            {
                db.CatalogItems.Add(item);
                db.SaveChanges();

                // Publish event — consumed by Stock.Worker via the event bus.
                // On ACA: Dapr publishes to Azure Service Bus topic "catalog-item-created".
                EventBus.Publish(new CatalogItemCreatedEvent
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    BrandId = item.CatalogBrandId,
                    TypeId = item.CatalogTypeId
                });

                return Created($"api/catalog/items/{item.Id}", item);
            }
        }

        [Route("items/{id:int}"), HttpPut]
        public IHttpActionResult UpdateItem(int id, CatalogItem updated)
        {
            using (var db = new CatalogDbContext())
            {
                var item = db.CatalogItems.Find(id);
                if (item == null) return NotFound();

                item.Name = updated.Name;
                item.Description = updated.Description;
                item.Price = updated.Price;
                item.PictureFileName = updated.PictureFileName;
                item.CatalogBrandId = updated.CatalogBrandId;
                item.CatalogTypeId = updated.CatalogTypeId;
                db.SaveChanges();

                EventBus.Publish(new CatalogItemUpdatedEvent
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Price = item.Price
                });

                return Ok(item);
            }
        }

        [Route("items/{id:int}"), HttpDelete]
        public IHttpActionResult DeleteItem(int id)
        {
            using (var db = new CatalogDbContext())
            {
                var item = db.CatalogItems.Find(id);
                if (item == null) return NotFound();

                db.CatalogItems.Remove(item);
                db.SaveChanges();

                // Stock.Worker reacts to this by cleaning up stock tracking state.
                EventBus.Publish(new CatalogItemDeletedEvent { ItemId = id });

                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
