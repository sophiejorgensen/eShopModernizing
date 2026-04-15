using System;

namespace eShop.Contracts.Events
{
    // Integration events published between services.
    // On .NET Framework: routed through IEventBus (in-memory or RabbitMQ).
    // After ACA migration: becomes Dapr pub/sub with Azure Service Bus.

    public class CatalogItemCreatedEvent
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class CatalogItemUpdatedEvent
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class CatalogItemDeletedEvent
    {
        public int ItemId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class StockUpdatedEvent
    {
        public int CatalogItemId { get; set; }
        public DateTime Date { get; set; }
        public int AvailableStock { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class LowStockDetectedEvent
    {
        public int CatalogItemId { get; set; }
        public string ItemName { get; set; }
        public int CurrentStock { get; set; }
        public DateTime Date { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class DiscountActivatedEvent
    {
        public int DiscountId { get; set; }
        public double Size { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
