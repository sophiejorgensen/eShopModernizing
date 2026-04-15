using System.Web.Http;

namespace Stock.Worker
{
    [RoutePrefix("health")]
    public class HealthController : ApiController
    {
        [Route(""), HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new { status = "Healthy", service = "Stock.Worker" });
        }
    }
}
