using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Gerente")]
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create(EmpleadoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var empleado = new hotelv1.Models.Entities.Empleado
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