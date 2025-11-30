using Microsoft.AspNetCore.Mvc;

namespace hotelv1.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            // Redirige a la página de login de Identity
            return Redirect($"/Identity/Account/Login?ReturnUrl={Uri.EscapeDataString(returnUrl)}");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return Redirect("/Identity/Account/Logout");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return Redirect("/Identity/Account/AccessDenied");
        }
    }
}
