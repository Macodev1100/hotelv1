using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hotelv1.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TwoFactorAuthenticationModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public bool Is2faEnabled { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Aquí podrías implementar la lógica para activar/desactivar 2FA
            StatusMessage = "Funcionalidad de 2FA no implementada.";
            return RedirectToPage();
        }
    }
}
