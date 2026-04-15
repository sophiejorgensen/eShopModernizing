using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;

namespace eShop.WebFrontend.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        // Hard-coded demo users (same as eShopOnWeb)
        private static readonly Dictionary<string, (string Password, string Role)> Users =
            new Dictionary<string, (string, string)>(System.StringComparer.OrdinalIgnoreCase)
            {
                ["demouser@microsoft.com"] = ("Pass@word1", "User"),
                ["admin@microsoft.com"] = ("Pass@word1", "Administrators"),
            };

        [HttpPost, Route("login")]
        public async Task<HttpResponseMessage> Login()
        {
            // Read form body
            var form = await Request.Content.ReadAsFormDataAsync();
            var email = form?["email"];
            var password = form?["password"];
            var returnUrl = form?["returnUrl"] ?? "/";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Redirect("/login.html?error=1&returnUrl=" + WebUtility.UrlEncode(returnUrl));

            if (!Users.TryGetValue(email, out var user) || user.Password != password)
                return Redirect("/login.html?error=1&returnUrl=" + WebUtility.UrlEncode(returnUrl));

            // Create claims identity and sign in
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
            var ctx = Request.GetOwinContext();
            ctx.Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            return Redirect(returnUrl);
        }

        [HttpPost, Route("logout")]
        public HttpResponseMessage Logout()
        {
            var ctx = Request.GetOwinContext();
            ctx.Authentication.SignOut("ApplicationCookie");
            return Redirect("/");
        }

        [HttpGet, Route("user")]
        public IHttpActionResult GetUser()
        {
            var principal = RequestContext.Principal as ClaimsPrincipal;
            if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
                return StatusCode(HttpStatusCode.Unauthorized);

            return Ok(new
            {
                email = principal.Identity.Name,
                role = principal.FindFirst(ClaimTypes.Role)?.Value ?? "User"
            });
        }

        private new HttpResponseMessage Redirect(string url)
        {
            var response = Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location = new System.Uri(url, System.UriKind.Relative);
            return response;
        }
    }
}
