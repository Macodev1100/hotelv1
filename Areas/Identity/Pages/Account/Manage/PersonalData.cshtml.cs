using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hotelv1.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PersonalDataModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }
            Username = await _userManager.GetUserNameAsync(user);
            Email = await _userManager.GetEmailAsync(user);
            PhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Aquí podrías implementar la descarga de datos personales
            StatusMessage = "Funcionalidad de descarga de datos no implementada.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se pudo cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }
            // Aquí podrías implementar la eliminación de la cuenta
            StatusMessage = "Funcionalidad de eliminación de cuenta no implementada.";
            return RedirectToPage();
        }
    }
}
