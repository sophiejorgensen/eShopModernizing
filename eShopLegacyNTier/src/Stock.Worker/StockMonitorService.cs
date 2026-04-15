using System;
using System.Data.Entity;
using System.Linq;
using eShop.Contracts.Events;
using eShop.EventBus;
using Stock.Worker.Data;

namespace Stock.Worker
{
    /// <summary>
    /// Subscribes to integration events and performs stock monitoring logic.
    /// 
    /// Each event subscription maps to a Dapr pub/sub topic subscription on ACA.
    /// The worker processes events asynchronously — exactly the pattern ACA scales on.
    /// </summary>
    public class StockMonitorService
    {
        private const int LowStockThreshold = 10;
        private readonly IEventBus _eventBus;

        private IDisposable _stockUpdatedSub;
        private IDisposable _catalogItemCreatedSub;
        private IDisposable _catalogItemDeletedSub;

        public StockMonitorService()
        {
            // In .NET Framework: shared in-memory event bus.
            // On ACA: Dapr subscribes to Service Bus topics automatically.
            _eventBus = EventBusInstance.Bus;
        }

        public void Start()
        {
            _stockUpdatedSub = _eventBus.Subscribe<StockUpdatedEvent>(OnStockUpdated);
            _catalogItemCreatedSub = _eventBus.Subscribe<CatalogItemCreatedEvent>(OnCatalogItemCreated);
            _catalogItemDeletedSub = _eventBus.Subscribe<CatalogItemDeletedEvent>(OnCatalogItemDeleted);

            Console.WriteLine("[Stock.Worker] Subscribed to: stock-updated, catalog-item-created, catalog-item-deleted");
        }

        public void Stop()
        {
            _stockUpdatedSub?.Dispose();
            _catalogItemCreatedSub?.Dispose();
            _catalogItemDeletedSub?.Dispose();

            Console.WriteLine("[Stock.Worker] Stopped.");
        }

        private void OnStockUpdated(StockUpdatedEvent evt)
        {
            Console.WriteLine($"[Stock.Worker] Stock updated: Item {evt.CatalogItemId}, " +
                              $"Date {evt.Date:d}, Available {evt.AvailableStock}");

            if (evt.AvailableStock < LowStockThreshold)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Stock.Worker] LOW STOCK ALERT: Item {evt.CatalogItemId} " +
                                  $"has only {evt.AvailableStock} units on {evt.Date:d}!");
                Console.ResetColor();

                // Publish low-stock alert — on ACA this goes to a notification service.
                _eventBus.Publish(new LowStockDetectedEvent
                {
                    CatalogItemId = evt.CatalogItemId,
                    CurrentStock = evt.AvailableStock,
                    Date = evt.Date
                });
            }
        }

        private void OnCatalogItemCreated(CatalogItemCreatedEvent evt)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Stock.Worker] New catalog item: {evt.Name} (ID {evt.ItemId}). " +
                              "Initializing stock tracking.");
            Console.ResetColor();

            using (var db = new StockDbContext())
            {
                var existingStock = db.CatalogItemStocks
                    .Any(s => s.CatalogItemId == evt.ItemId);

                if (!existingStock)
                {
                    db.CatalogItemStocks.Add(new eShop.Contracts.Models.CatalogItemStock
                    {
                        CatalogItemId = evt.ItemId,
                        Date = DateTime.UtcNow.Date,
                        AvailableStock = 0
                    });
                    db.SaveChanges();
                    Console.WriteLine($"[Stock.Worker] Initial stock entry created for Item {evt.ItemId}.");
                }
            }
        }

        private void OnCatalogItemDeleted(CatalogItemDeletedEvent evt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Stock.Worker] Catalog item deleted: {evt.ItemId}. Cleaning up stock entries.");
            Console.ResetColor();

            using (var db = new StockDbContext())
            {
                var stocks = db.CatalogItemStocks
                    .Where(s => s.CatalogItemId == evt.ItemId)
                    .ToList();

                if (stocks.Any())
                {
                    db.CatalogItemStocks.RemoveRange(stocks);
                    db.SaveChanges();
                    Console.WriteLine($"[Stock.Worker] Removed {stocks.Count} stock entries for Item {evt.ItemId}.");
                }
            }
        }
    }
}
