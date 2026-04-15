using System;
using Microsoft.Owin.Hosting;

namespace Discount.Service
{
    /// <summary>
    /// Self-hosted Discount API service.
    /// 
    /// On ACA: this becomes an INTERNAL container app (no public ingress).
    /// Catalog.Service calls it via Dapr service invocation using just the app-id.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:5002";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("============================================");
                Console.WriteLine("  Discount Service");
                Console.WriteLine($"  Listening on {url}");
                Console.WriteLine("============================================");
                Console.WriteLine();
                Console.WriteLine("  [ACA mapping: discount-api container app]");
                Console.WriteLine("  [Ingress: INTERNAL only (no public URL)]");
                Console.WriteLine("  [Called by: Catalog.Service via Dapr]");
                Console.WriteLine();
                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}
