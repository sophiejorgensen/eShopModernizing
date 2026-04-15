using System;

namespace eShop.EventBus
{
    /// <summary>
    /// Abstraction for inter-service messaging.
    /// 
    /// .NET Framework: InMemoryEventBus (single-process demo) or RabbitMqEventBus (distributed).
    /// After ACA migration: replaced by Dapr pub/sub — no code changes to publishers/subscribers.
    /// </summary>
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : class;
        IDisposable Subscribe<T>(Action<T> handler) where T : class;
    }
}
