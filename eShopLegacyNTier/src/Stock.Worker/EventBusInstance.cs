using eShop.EventBus;

namespace Stock.Worker
{
    public static class EventBusInstance
    {
        public static readonly IEventBus Bus = new InMemoryEventBus();
    }
}
