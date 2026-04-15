using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

namespace Catalog.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Catalog Service API");
                c.DescribeAllEnumsAsStrings();
            }).EnableSwaggerUi();

            app.UseWebApi(config);

            // Serve the dashboard UI from wwwroot folder
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(wwwroot))
                wwwroot = Path.Combine(Path.GetDirectoryName(typeof(Startup).Assembly.Location), "wwwroot");

            if (Directory.Exists(wwwroot))
            {
                var fileSystem = new PhysicalFileSystem(wwwroot);
                var options = new FileServerOptions
                {
                    FileSystem = fileSystem,
                    EnableDefaultFiles = true
                };
                options.DefaultFilesOptions.DefaultFileNames.Add("index.html");
                app.UseFileServer(options);
            }
        }
    }
}
