using eShop.EventBus;

namespace Catalog.Service
{
    /// <summary>
    /// Shared event bus instance.
    /// 
    /// .NET Framework: static singleton (services communicate in-process or via RabbitMQ).
    /// After ACA migration: replaced by Dapr DaprClient injected via DI — this class goes away.
    /// </summary>
    public static class EventBusInstance
    {
        public static readonly IEventBus Bus = new InMemoryEventBus();
    }
}
