using System;
using Microsoft.Owin.Hosting;

namespace eShop.WebFrontend
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:5000";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("============================================");
                Console.WriteLine("  eShop Web Storefront");
                Console.WriteLine("  Listening on {0}", url);
                Console.WriteLine("============================================");
                Console.WriteLine();
                Console.WriteLine("  [ACA mapping: web-frontend container app]");
                Console.WriteLine("  [Ingress: external (public)]");
                Console.WriteLine();
                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}
