using System;
using Microsoft.Owin.Hosting;

namespace Stock.Worker
{
    /// <summary>
    /// Event-driven stock monitoring worker.
    /// 
    /// On .NET Framework: runs as a console app / Windows Service, subscribes to events via IEventBus.
    /// On ACA: container app with minReplicas=0, scales to zero when no events flow.
    ///         Dapr subscription replaces IEventBus. KEDA scales based on Service Bus queue depth.
    /// 
    /// This is the #1 reason ACA beats App Service — background workers that cost $0 when idle.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("  Stock Worker");
            Console.WriteLine("============================================");
            Console.WriteLine();
            Console.WriteLine("  [ACA mapping: stock-worker container app]");
            Console.WriteLine("  [Scaling: 0-5 replicas based on queue depth]");
            Console.WriteLine("  [Scale-to-zero: YES — $0 when idle]");
            Console.WriteLine();

            var monitor = new StockMonitorService();
            monitor.Start();

            Data.StockDbContext.EnsureSeedData();
            Console.WriteLine("Stock database initialized.");

            using (WebApp.Start<Startup>("http://localhost:5003"))
            {
                Console.WriteLine("Health endpoint listening on http://localhost:5003/health");
                Console.WriteLine("Listening for events. Press Enter to stop...");
                Console.ReadLine();
            }

            monitor.Stop();
        }
    }
}
