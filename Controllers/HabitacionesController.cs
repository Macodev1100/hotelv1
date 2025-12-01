using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace hotelv1.Controllers
{
    // Allow Limpieza/Mantenimiento to access Index and ToggleEstado only
    [Authorize]
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
            // Validar número único
            if (_context.Habitaciones.Any(h => h.Numero == model.Numero))
            {
                ModelState.AddModelError("Numero", "Ya existe una habitación con ese número.");
            }

            // Procesar archivo de foto si se subió
            var file = Request.Form.Files["FotoFile"];
            string fileName = null;
            if (file != null && file.Length > 0)
            {
                // Guardar en wwwroot/img/habitaciones
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/habitaciones");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                model.FotoUrl = "/img/habitaciones/" + fileName;
            }

            if (ModelState.IsValid)
            {
                var habitacion = new hotelv1.Models.Entities.Habitacion
                {
                    Numero = model.Numero,
                    Tipo = model.Tipo,
                    PrecioPorNoche = model.PrecioPorNoche,
                    Disponible = model.Disponible,
                    Descripcion = model.Descripcion,
                    ObservacionEstado = model.ObservacionEstado,
                    Ocupada = model.Ocupada,
                    FotoUrl = model.FotoUrl
                };
                _context.Habitaciones.Add(habitacion);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // Recargar tipos disponibles si hay error
            model.TiposDisponibles = new System.Collections.Generic.List<string> { "Suite", "Doble", "Individual", "Familiar" };
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
                Descripcion = habitacion.Descripcion,
                ObservacionEstado = habitacion.ObservacionEstado,
                Ocupada = habitacion.Ocupada,
                FotoUrl = habitacion.FotoUrl,
                TiposDisponibles = new System.Collections.Generic.List<string> { "Suite", "Doble", "Individual", "Familiar" }
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
            // Validar número único (excepto la misma habitación)
            if (_context.Habitaciones.Any(h => h.Numero == model.Numero && h.HabitacionId != id))
            {
                ModelState.AddModelError("Numero", "Ya existe una habitación con ese número.");
            }
            if (ModelState.IsValid)
            {
                habitacion.Numero = model.Numero;
                habitacion.Tipo = model.Tipo;
                habitacion.PrecioPorNoche = model.PrecioPorNoche;
                habitacion.Disponible = model.Disponible;
                habitacion.Descripcion = model.Descripcion;
                habitacion.ObservacionEstado = model.ObservacionEstado;
                habitacion.Ocupada = model.Ocupada;
                habitacion.FotoUrl = model.FotoUrl;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // Recargar tipos disponibles si hay error
            model.TiposDisponibles = new System.Collections.Generic.List<string> { "Suite", "Doble", "Individual", "Familiar" };
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