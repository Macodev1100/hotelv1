
using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Models;
using hotelv1.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Gerente")]
    public class ReservasController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var reservas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Habitacion).ToList();
            return View(reservas);
        }


        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create()
        {
            var model = new ReservaViewModel
            {
                Clientes = _context.Clientes.ToList(),
                Habitaciones = _context.Habitaciones.Where(h => h.Disponible).ToList(),
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1),
                Estado = "Pendiente"
            };
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create(ReservaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var habitacion = _context.Habitaciones.FirstOrDefault(h => h.HabitacionId == model.HabitacionId);
                var dias = (model.FechaSalida - model.FechaEntrada).Days;
                if (dias < 1) dias = 1;
                var total = habitacion != null ? habitacion.PrecioPorNoche * dias : 0;
                var reserva = new hotelv1.Models.Entities.Reserva
                {
                    ClienteId = model.ClienteId,
                    HabitacionId = model.HabitacionId,
                    FechaEntrada = model.FechaEntrada,
                    FechaSalida = model.FechaSalida,
                    Estado = model.Estado,
                    Total = total,
                    MetodoPago = model.MetodoPago
                };
                _context.Reservas.Add(reserva);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Clientes = _context.Clientes.ToList();
            model.Habitaciones = _context.Habitaciones.Where(h => h.Disponible).ToList();
            return View(model);
        }


        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound();
            var model = new ReservaViewModel
            {
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                Estado = reserva.Estado,
                Total = reserva.Total,
                MetodoPago = reserva.MetodoPago,
                Clientes = _context.Clientes.ToList(),
                Habitaciones = _context.Habitaciones.Where(h => h.Disponible || h.HabitacionId == reserva.HabitacionId).ToList()
            };
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id, ReservaViewModel model)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                var habitacion = _context.Habitaciones.FirstOrDefault(h => h.HabitacionId == model.HabitacionId);
                var dias = (model.FechaSalida - model.FechaEntrada).Days;
                if (dias < 1) dias = 1;
                var total = habitacion != null ? habitacion.PrecioPorNoche * dias : 0;
                reserva.ClienteId = model.ClienteId;
                reserva.HabitacionId = model.HabitacionId;
                reserva.FechaEntrada = model.FechaEntrada;
                reserva.FechaSalida = model.FechaSalida;
                reserva.Estado = model.Estado;
                reserva.Total = total;
                reserva.MetodoPago = model.MetodoPago;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Clientes = _context.Clientes.ToList();
            model.Habitaciones = _context.Habitaciones.Where(h => h.Disponible || h.HabitacionId == reserva.HabitacionId).ToList();
            return View(model);
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var reserva = _context.Reservas.Include(r => r.Cliente).Include(r => r.Habitacion).FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
                return NotFound();
            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}