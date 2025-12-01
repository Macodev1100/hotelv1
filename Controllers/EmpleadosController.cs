using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using hotelv1.Models.Entities;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Gerente")]
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpleadosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var empleados = _context.Empleados.ToList();
            return View(empleados);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View(new EmpleadoViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(EmpleadoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el usuario ya existe
                var existingUser = await _userManager.FindByEmailAsync(model.Email ?? string.Empty);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Ya existe un usuario con ese correo electrónico.");
                    return View(model);
                }

                // Crear usuario en Identity
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Asignar rol
                var role = model.Puesto;
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);

                // Guardar empleado en la base de datos
                var empleado = new Empleado
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Puesto = model.Puesto,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    FechaContratacion = model.FechaContratacion,
                    Activo = model.Activo
                };
                _context.Empleados.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador,Gerente")]
        public IActionResult Edit(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
                return NotFound();
            var model = new EmpleadoViewModel
            {
                EmpleadoId = empleado.EmpleadoId,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Puesto = empleado.Puesto,
                Telefono = empleado.Telefono,
                Email = empleado.Email,
                FechaContratacion = empleado.FechaContratacion,
                Activo = empleado.Activo
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Gerente")]
        public IActionResult Edit(int id, EmpleadoViewModel model)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                empleado.Nombre = model.Nombre;
                empleado.Apellido = model.Apellido;
                empleado.Puesto = model.Puesto;
                empleado.Telefono = model.Telefono;
                empleado.Email = model.Email;
                empleado.FechaContratacion = model.FechaContratacion;
                empleado.Activo = model.Activo;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
                return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}