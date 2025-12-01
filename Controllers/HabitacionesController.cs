
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using hotelv1.Data;
using hotelv1.ViewModels;

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

        [Authorize(Roles = "Mantenimiento")]
        public IActionResult EditEstado(int id)
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
                ObservacionEstado = habitacion.ObservacionEstado
            };
            return View("EditEstado", model);
        }

        [HttpPost]
        [Authorize(Roles = "Mantenimiento")]
        public IActionResult EditEstado(HabitacionViewModel model)
        {
            var habitacion = _context.Habitaciones.Find(model.HabitacionId);
            if (habitacion == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                habitacion.Disponible = model.Disponible;
                habitacion.ObservacionEstado = model.ObservacionEstado;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // Solo mostrar los campos permitidos
            model.Numero = habitacion.Numero;
            model.Tipo = habitacion.Tipo;
            model.PrecioPorNoche = habitacion.PrecioPorNoche;
            return View("EditEstado", model);
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
            string? fileName = null;
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
            // Procesar archivo de foto si se subió uno nuevo
            var file = Request.Form.Files["FotoFile"];
            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/habitaciones");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                habitacion.FotoUrl = "/img/habitaciones/" + fileName;
            }
            else
            {
                // Si no se sube nueva foto, mantener la actual
                habitacion.FotoUrl = model.FotoUrl;
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
        [Authorize(Roles = "Mantenimiento")]
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