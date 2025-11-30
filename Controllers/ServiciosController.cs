using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Empleado,Gerente")]
    public class ServiciosController : Controller
    {
        private static List<hotelv1.Models.Entities.Servicio> servicios = new List<hotelv1.Models.Entities.Servicio>();

        public IActionResult Index()
        {
            return View(servicios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(hotelv1.Models.Entities.Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                servicio.ServicioId = servicios.Count + 1;
                servicios.Add(servicio);
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        public IActionResult Edit(int id)
        {
            var servicio = servicios.Find(s => s.ServicioId == id);
            if (servicio == null)
                return NotFound();
            return View(servicio);
        }

        [HttpPost]
        public IActionResult Edit(int id, hotelv1.Models.Entities.Servicio servicio)
        {
            var original = servicios.Find(s => s.ServicioId == id);
            if (original == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                original.Nombre = servicio.Nombre;
                original.Descripcion = servicio.Descripcion;
                original.Precio = servicio.Precio;
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        public IActionResult Delete(int id)
        {
            var servicio = servicios.Find(s => s.ServicioId == id);
            if (servicio == null)
                return NotFound();
            return View(servicio);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var servicio = servicios.Find(s => s.ServicioId == id);
            if (servicio != null)
                servicios.Remove(servicio);
            return RedirectToAction("Index");
        }
    }
}