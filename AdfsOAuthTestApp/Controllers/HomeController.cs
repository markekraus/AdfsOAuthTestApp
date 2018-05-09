using AdfsOAuthTestApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AdfsOAuthTestApp.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;

        public HomeController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            ViewData["AdfsLogoutUrl"] = _configuration["AdfsLogoutUrl"];
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize]
        public IActionResult Secure()
        {
            ViewData["AdfsLogoutUrl"] = _configuration["AdfsLogoutUrl"];
            ViewData["UPN"] = User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").FirstOrDefault().Value;
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Signout()
        {
            ViewData["AdfsLogoutUrl"] = _configuration["AdfsLogoutUrl"];
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "Cookies");
            return Redirect("/");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
