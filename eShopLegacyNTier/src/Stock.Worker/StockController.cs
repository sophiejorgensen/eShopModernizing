using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Stock.Worker.Data;

namespace Stock.Worker
{
    [RoutePrefix("api/stock")]
    public class StockController : ApiController
    {
        [Route(""), HttpGet]
        public IHttpActionResult GetAll()
        {
            using (var db = new StockDbContext())
            {
                var stocks = db.CatalogItemStocks
                    .OrderBy(s => s.CatalogItemId)
                    .ToList();
                return Ok(stocks);
            }
        }

        [Route("{catalogItemId:int}"), HttpGet]
        public IHttpActionResult GetByItem(int catalogItemId)
        {
            using (var db = new StockDbContext())
            {
                var stock = db.CatalogItemStocks
                    .Where(s => s.CatalogItemId == catalogItemId)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if (stock == null) return NotFound();
                return Ok(stock);
            }
        }

        [Route("{catalogItemId:int}/decrement"), HttpPost]
        public IHttpActionResult Decrement(int catalogItemId, [FromBody] DecrementRequest request)
        {
            int qty = request?.Quantity ?? 1;
            using (var db = new StockDbContext())
            {
                var stock = db.CatalogItemStocks
                    .Where(s => s.CatalogItemId == catalogItemId)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if (stock == null) return NotFound();
                stock.AvailableStock = System.Math.Max(0, stock.AvailableStock - qty);
                db.SaveChanges();
                return Ok(stock);
            }
        }
    }

    public class DecrementRequest
    {
        public int Quantity { get; set; }
    }
}
