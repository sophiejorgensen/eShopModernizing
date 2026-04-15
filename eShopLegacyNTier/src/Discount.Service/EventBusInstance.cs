using eShop.EventBus;

namespace Discount.Service
{
    public static class EventBusInstance
    {
        public static readonly IEventBus Bus = new InMemoryEventBus();
    }
}
