using System.IO;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace eShop.WebFrontend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ── Cookie authentication ──
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                CookieName = "eShopIdentifier",
                LoginPath = new PathString("/login.html"),
                ExpireTimeSpan = System.TimeSpan.FromMinutes(60),
                SlidingExpiration = true,
            });

            // ── Web API (account endpoints) ──
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            // ── Static files ──
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(wwwroot))
                wwwroot = Path.Combine(Path.GetDirectoryName(typeof(Startup).Assembly.Location), "wwwroot");

            if (Directory.Exists(wwwroot))
            {
                var fileSystem = new PhysicalFileSystem(wwwroot);
                var options = new FileServerOptions
                {
                    FileSystem = fileSystem,
                    EnableDefaultFiles = true,
                    EnableDirectoryBrowsing = false
                };
                options.DefaultFilesOptions.DefaultFileNames.Add("index.html");
                options.StaticFileOptions.ServeUnknownFileTypes = true;
                app.UseFileServer(options);
            }
        }
    }
}
