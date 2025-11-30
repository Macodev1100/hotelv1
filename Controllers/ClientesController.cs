using Microsoft.AspNetCore.Mvc;
using hotelv1.ViewModels;
using hotelv1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace hotelv1.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista,Gerente")]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clientes = _context.Clientes.ToList();
            return View(clientes);
        }

        [Authorize(Roles = "Administrador,Recepcionista")] // Solo admin y recepcionista pueden crear
        public IActionResult Create()
        {
            return View(new ClienteViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Create(ClienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cliente = new hotelv1.Models.Entities.Cliente
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    Direccion = model.Direccion,
                    FechaRegistro = model.FechaRegistro,
                    Activo = model.Activo
                };
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return NotFound();
            var model = new ClienteViewModel
            {
                ClienteId = cliente.ClienteId,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                FechaRegistro = cliente.FechaRegistro,
                Activo = cliente.Activo
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Recepcionista")]
        public IActionResult Edit(int id, ClienteViewModel model)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                cliente.Nombre = model.Nombre;
                cliente.Apellido = model.Apellido;
                cliente.Telefono = model.Telefono;
                cliente.Email = model.Email;
                cliente.Direccion = model.Direccion;
                cliente.FechaRegistro = model.FechaRegistro;
                cliente.Activo = model.Activo;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}