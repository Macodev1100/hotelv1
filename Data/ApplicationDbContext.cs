using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hotelv1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<hotelv1.Models.Entities.Cliente> Clientes { get; set; }
        public DbSet<hotelv1.Models.Entities.Empleado> Empleados { get; set; }
        public DbSet<hotelv1.Models.Entities.Habitacion> Habitaciones { get; set; }
        public DbSet<hotelv1.Models.Entities.Reserva> Reservas { get; set; }
        public DbSet<hotelv1.Models.Entities.Servicio> Servicios { get; set; }
        public DbSet<hotelv1.Models.Entities.Factura> Facturas { get; set; }
        public DbSet<hotelv1.Models.Entities.ReservaServicio> ReservaServicios { get; set; }
    }
}
