using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Discount.Service.Data;
using eShop.Contracts.Events;
using eShop.Contracts.Models;
using eShop.EventBus;

namespace Discount.Service.Controllers
{
    [RoutePrefix("api/discounts")]
    public class DiscountController : ApiController
    {
        private static readonly IEventBus EventBus = EventBusInstance.Bus;

        [Route("active"), HttpGet]
        public IHttpActionResult GetActiveDiscount(DateTime? date = null)
        {
            var targetDate = (date ?? DateTime.UtcNow).Date;

            using (var db = new DiscountDbContext())
            {
                var discount = db.DiscountItems
                    .FirstOrDefault(d => DbFunctions.TruncateTime(d.Start) <= targetDate
                                     && DbFunctions.TruncateTime(d.End) >= targetDate);

                if (discount == null) return StatusCode(System.Net.HttpStatusCode.NoContent);
                return Ok(discount);
            }
        }

        [Route(""), HttpGet]
        public IHttpActionResult GetAll()
        {
            using (var db = new DiscountDbContext())
            {
                return Ok(db.DiscountItems.OrderBy(d => d.Start).ToList());
            }
        }

        [Route(""), HttpPost]
        public IHttpActionResult CreateDiscount(DiscountItem discount)
        {
            using (var db = new DiscountDbContext())
            {
                db.DiscountItems.Add(discount);
                db.SaveChanges();

                // Publish event — other services can react (e.g., notification service).
                // On ACA: Dapr publishes to "discount-activated" topic.
                EventBus.Publish(new DiscountActivatedEvent
                {
                    DiscountId = discount.Id,
                    Size = discount.Size,
                    Start = discount.Start,
                    End = discount.End
                });

                return Created($"api/discounts/{discount.Id}", discount);
            }
        }

        [Route("{id:int}"), HttpDelete]
        public IHttpActionResult DeleteDiscount(int id)
        {
            using (var db = new DiscountDbContext())
            {
                var discount = db.DiscountItems.Find(id);
                if (discount == null) return NotFound();

                db.DiscountItems.Remove(discount);
                db.SaveChanges();
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
