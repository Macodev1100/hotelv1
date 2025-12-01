using hotelv1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotelv1.Data
{
    public static class SeedDemoData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Habitaciones.Any())
            {
                var habitaciones = new List<Habitacion>();
                for (int i = 1; i <= 50; i++)
                {
                    habitaciones.Add(new Habitacion
                    {
                        Numero = (100 + i).ToString(),
                        Tipo = i % 4 == 0 ? "Suite" : i % 3 == 0 ? "Familiar" : i % 2 == 0 ? "Doble" : "Individual",
                        PrecioPorNoche = 50 + (i % 4) * 30,
                        Disponible = i % 5 != 0,
                        Descripcion = $"Habitación de prueba número {i}",
                        ObservacionEstado = i % 7 == 0 ? "En mantenimiento" : null,
                        Ocupada = i % 6 == 0,
                        FotoUrl = null
                    });
                }
                context.Habitaciones.AddRange(habitaciones);
            }

            if (!context.Clientes.Any())
            {
                var clientes = new List<Cliente>();
                for (int i = 1; i <= 30; i++)
                {
                    clientes.Add(new Cliente
                    {
                        Nombre = $"Cliente{i}",
                        Apellido = $"Apellido{i}",
                        Email = $"cliente{i}@demo.com",
                        Telefono = $"099000{i:0000}",
                        Direccion = $"Calle {i} #123"
                    });
                }
                context.Clientes.AddRange(clientes);
            }

            if (!context.Empleados.Any())
            {
                var empleados = new List<Empleado>();
                for (int i = 1; i <= 10; i++)
                {
                    empleados.Add(new Empleado
                    {
                        Nombre = $"Empleado{i}",
                        Apellido = $"Apellido{i}",
                        Email = $"empleado{i}@hotel.com",
                        Telefono = $"098100{i:0000}",
                        Puesto = i % 2 == 0 ? "Recepcionista" : "Limpieza"
                    });
                }
                context.Empleados.AddRange(empleados);
            }

            if (!context.Servicios.Any())
            {
                var servicios = new List<Servicio>
                {
                    new Servicio { Nombre = "Desayuno", Precio = 10 },
                    new Servicio { Nombre = "Almuerzo", Precio = 18 },
                    new Servicio { Nombre = "Spa", Precio = 25 },
                    new Servicio { Nombre = "Lavandería", Precio = 8 },
                    new Servicio { Nombre = "Transporte", Precio = 15 }
                };
                context.Servicios.AddRange(servicios);
            }

            await context.SaveChangesAsync();
        }

        public static async Task DeleteAllAsync(ApplicationDbContext context)
        {
            context.Reservas.RemoveRange(context.Reservas);
            context.Facturas.RemoveRange(context.Facturas);
            context.Habitaciones.RemoveRange(context.Habitaciones);
            context.Clientes.RemoveRange(context.Clientes);
            context.Empleados.RemoveRange(context.Empleados);
            context.Servicios.RemoveRange(context.Servicios);
            await context.SaveChangesAsync();
        }
    }
}
