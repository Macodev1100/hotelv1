using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace hotelv1.Data
{
    public static class SeedUsers
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Usuarios y roles
            var users = new[]
            {
                new { Email = "admin@hotel.com", Password = "Admin123*", Role = "Administrador" },
                new { Email = "recepcion@hotel.com", Password = "Recep123*", Role = "Recepcionista" },
                new { Email = "empleado@hotel.com", Password = "Empleado123*", Role = "Empleado" },
                new { Email = "gerente@hotel.com", Password = "Gerente123*", Role = "Gerente" }
            };

            foreach (var u in users)
            {
                var user = await userManager.FindByEmailAsync(u.Email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = u.Email, Email = u.Email, EmailConfirmed = true };
                    await userManager.CreateAsync(user, u.Password);
                }
                if (!await userManager.IsInRoleAsync(user, u.Role))
                {
                    if (!await roleManager.RoleExistsAsync(u.Role))
                        await roleManager.CreateAsync(new IdentityRole(u.Role));
                    await userManager.AddToRoleAsync(user, u.Role);
                }
            }
        }
    }
}
