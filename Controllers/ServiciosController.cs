using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Empleado,Gerente")]
    public class ServiciosController : Controller
    {
        private readonly hotelv1.Data.ApplicationDbContext _context;

        public ServiciosController(hotelv1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var servicios = _context.Servicios.ToList();
            return View(servicios);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create(hotelv1.Models.Entities.Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Servicios.Add(servicio);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
                return NotFound();
            return View(servicio);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, hotelv1.Models.Entities.Servicio servicio)
        {
            var original = _context.Servicios.Find(id);
            if (original == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                original.Nombre = servicio.Nombre;
                original.Descripcion = servicio.Descripcion;
                original.Precio = servicio.Precio;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
                return NotFound();
            return View(servicio);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}