using System.Diagnostics;
using hotelv1.Models;
using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace hotelv1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly hotelv1.Data.ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, hotelv1.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            // Datos reales
            var hoy = DateTime.Today;
            ViewBag.Habitaciones = _context.Habitaciones.Count();
            ViewBag.HabitacionesDisponibles = _context.Habitaciones.Count(h => h.Disponible);
            ViewBag.Clientes = _context.Clientes.Count();
            ViewBag.Empleados = _context.Empleados.Count();
            ViewBag.Reservas = _context.Reservas.Count();
            ViewBag.Facturacion = _context.Facturas.Sum(f => (decimal?)f.MontoTotal) ?? 0;

            // Distribución de habitaciones por tipo
            var distribucion = _context.Habitaciones
                .GroupBy(h => h.Tipo)
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .ToList();
            ViewBag.HabitacionesDistribucionTipos = distribucion;

            // Check-ins de hoy
            var checkinsHoy = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Where(r => r.FechaEntrada == hoy)
                .ToList();
            ViewBag.CheckinsHoy = checkinsHoy;

            // Próximos check-ins (próximos 3)
            var proximosCheckins = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Where(r => r.FechaEntrada >= hoy)
                .OrderBy(r => r.FechaEntrada)
                .Take(3)
                .ToList();
            ViewBag.ProximosCheckins = proximosCheckins;

            // Ranking habitaciones por reservas
            var rankingHabitaciones = _context.Reservas
                .GroupBy(r => r.HabitacionId)
                .Select(g => new {
                    HabitacionId = g.Key,
                    TotalReservas = g.Count()
                })
                .OrderByDescending(x => x.TotalReservas)
                .Take(4)
                .ToList();
            var habitaciones = _context.Habitaciones.ToList();
            ViewBag.RankingHabitaciones = rankingHabitaciones
                .Select(r => new {
                    Nombre = habitaciones.FirstOrDefault(h => h.HabitacionId == r.HabitacionId)?.Numero + " (" + habitaciones.FirstOrDefault(h => h.HabitacionId == r.HabitacionId)?.Tipo + ")",
                    Reservas = r.TotalReservas
                }).ToList();

            // Notificaciones recientes (simples)
            var notificaciones = new List<string>();
            if (checkinsHoy.Any())
                notificaciones.Add($"Check-in realizado: <b>{checkinsHoy.First().Cliente?.Nombre} {checkinsHoy.First().Cliente?.Apellido}</b> ({checkinsHoy.First().Habitacion?.Numero})");
            if (proximosCheckins.Any())
                notificaciones.Add($"Reserva pendiente: <b>{proximosCheckins.First().Cliente?.Nombre} {proximosCheckins.First().Cliente?.Apellido}</b> ({proximosCheckins.First().Habitacion?.Numero})");
            notificaciones.Add("Habitación <b>" + (habitaciones.FirstOrDefault(h => !h.Disponible)?.Numero ?? "-") + "</b> lista para limpieza");
            notificaciones.Add("Nuevo cliente registrado: <b>" + (_context.Clientes.OrderByDescending(c => c.ClienteId).FirstOrDefault()?.Nombre ?? "-") + "</b>");
            ViewBag.Notificaciones = notificaciones;

            // Ocupación actual
            var ocupadas = _context.Habitaciones.Count(h => !h.Disponible);
            var libres = ViewBag.HabitacionesDisponibles;
            var total = ViewBag.Habitaciones;
            ViewBag.OcupacionPorc = total > 0 ? (int)Math.Round((double)ocupadas * 100 / total) : 0;
            ViewBag.Ocupadas = ocupadas;
            ViewBag.Libres = libres;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Configuracion()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargarDemo()
        {
            await hotelv1.Data.SeedDemoData.SeedAsync(_context);
            TempData["DemoMsg"] = "Datos demo cargados correctamente.";
            return RedirectToAction("Configuracion");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDemo()
        {
            await hotelv1.Data.SeedDemoData.DeleteAllAsync(_context);
            TempData["DemoMsg"] = "Datos demo eliminados correctamente.";
            return RedirectToAction("Configuracion");
        }
    }
}
