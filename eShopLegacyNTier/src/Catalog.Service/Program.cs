using System;
using Microsoft.Owin.Hosting;

namespace Catalog.Service
{
    /// <summary>
    /// Self-hosted Catalog API service.
    /// 
    /// Each service runs as its own process — on ACA each becomes its own Container App
    /// with independent scaling rules and health checks.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:5001";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("============================================");
                Console.WriteLine("  Catalog Service");
                Console.WriteLine($"  Listening on {url}");
                Console.WriteLine("  Swagger: {0}/swagger", url);
                Console.WriteLine("============================================");
                Console.WriteLine();
                Console.WriteLine("  [ACA mapping: catalog-api container app]");
                Console.WriteLine("  [Scaling: HTTP concurrent requests]");
                Console.WriteLine();
                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}
