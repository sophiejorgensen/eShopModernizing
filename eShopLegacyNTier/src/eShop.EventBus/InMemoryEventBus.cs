using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace eShop.EventBus
{
    /// <summary>
    /// In-memory event bus for local development and demos.
    /// Works within a single process — all services share this instance.
    /// 
    /// For distributed deployment (multiple processes/containers), swap to:
    ///   - RabbitMqEventBus (pre-ACA)
    ///   - Dapr pub/sub (on ACA) — this entire class goes away
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers
            = new ConcurrentDictionary<Type, List<Delegate>>();

        public void Publish<T>(T @event) where T : class
        {
            List<Delegate> handlers;
            if (_handlers.TryGetValue(typeof(T), out handlers))
            {
                foreach (var handler in handlers.ToList())
                {
                    try
                    {
                        ((Action<T>)handler)(@event);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[EventBus] Error handling {typeof(T).Name}: {ex.Message}");
                    }
                }
            }
        }

        public IDisposable Subscribe<T>(Action<T> handler) where T : class
        {
            var handlers = _handlers.GetOrAdd(typeof(T), _ => new List<Delegate>());
            lock (handlers)
            {
                handlers.Add(handler);
            }
            return new Subscription(() =>
            {
                lock (handlers)
                {
                    handlers.Remove(handler);
                }
            });
        }

        private class Subscription : IDisposable
        {
            private readonly Action _onDispose;
            public Subscription(Action onDispose) { _onDispose = onDispose; }
            public void Dispose() { _onDispose(); }
        }
    }
}
