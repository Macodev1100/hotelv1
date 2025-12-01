using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Gerente")]
    public class HabitacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HabitacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var habitaciones = _context.Habitaciones.ToList();
            return View(habitaciones);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View(new HabitacionViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create(HabitacionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var habitacion = new hotelv1.Models.Entities.Habitacion
                {
                    Numero = model.Numero,
                    Tipo = model.Tipo,
                    PrecioPorNoche = model.PrecioPorNoche,
                    Disponible = model.Disponible,
                    Descripcion = model.Descripcion
                };
                _context.Habitaciones.Add(habitacion);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var habitacion = _context.Habitaciones.Find(id);
            if (habitacion == null)
                return NotFound();
            var model = new HabitacionViewModel
            {
                HabitacionId = habitacion.HabitacionId,
                Numero = habitacion.Numero,
                Tipo = habitacion.Tipo,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Disponible = habitacion.Disponible,
                Descripcion = habitacion.Descripcion
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, HabitacionViewModel model)
        {
            var habitacion = _context.Habitaciones.Find(id);
            if (habitacion == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                habitacion.Numero = model.Numero;
                habitacion.Tipo = model.Tipo;
                habitacion.PrecioPorNoche = model.PrecioPorNoche;
                habitacion.Disponible = model.Disponible;
                habitacion.Descripcion = model.Descripcion;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var habitacion = _context.Habitaciones.Find(id);
            if (habitacion == null)
                return NotFound();
            return View(habitacion);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var habitacion = _context.Habitaciones.Find(id);
            if (habitacion != null)
            {
                _context.Habitaciones.Remove(habitacion);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Limpieza,Mantenimiento")]
        public IActionResult ToggleEstado(int id)
        {
            var habitacion = _context.Habitaciones.Find(id);
            if (habitacion == null)
                return NotFound();
            habitacion.Disponible = !habitacion.Disponible;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}