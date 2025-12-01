using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Gerente")]
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var facturas = _context.Facturas.ToList();
            return View(facturas);
        }

        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create()
        {
            var reservas = _context.Reservas.ToList();
            return View(new FacturaViewModel { FechaEmision = DateTime.Today, Reservas = reservas });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create(FacturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validar que la reserva existe
                var reserva = _context.Reservas.FirstOrDefault(r => r.ReservaId == model.ReservaId);
                if (reserva == null)
                {
                    ModelState.AddModelError("ReservaId", "La reserva seleccionada no existe.");
                }
                else
                {
                    var factura = new hotelv1.Models.Entities.Factura
                    {
                        ReservaId = model.ReservaId,
                        FechaEmision = model.FechaEmision,
                        MontoTotal = model.MontoTotal,
                        Detalle = model.Detalle
                    };
                    _context.Facturas.Add(factura);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            model.Reservas = _context.Reservas.ToList();
            return View(model);
        }

        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id)
        {
            var factura = _context.Facturas.Find(id);
            if (factura == null)
                return NotFound();
            var model = new FacturaViewModel
            {
                FacturaId = factura.FacturaId,
                ReservaId = factura.ReservaId,
                FechaEmision = factura.FechaEmision,
                MontoTotal = factura.MontoTotal,
                Detalle = factura.Detalle,
                Reservas = _context.Reservas.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id, FacturaViewModel model)
        {
            var factura = _context.Facturas.Find(id);
            if (factura == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                var reserva = _context.Reservas.FirstOrDefault(r => r.ReservaId == model.ReservaId);
                if (reserva == null)
                {
                    ModelState.AddModelError("ReservaId", "La reserva seleccionada no existe.");
                }
                else
                {
                    factura.ReservaId = model.ReservaId;
                    factura.FechaEmision = model.FechaEmision;
                    factura.MontoTotal = model.MontoTotal;
                    factura.Detalle = model.Detalle;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            model.Reservas = _context.Reservas.ToList();
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var factura = _context.Facturas.Find(id);
            if (factura == null)
                return NotFound();
            return View(factura);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var factura = _context.Facturas.Find(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}