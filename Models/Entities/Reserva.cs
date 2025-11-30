using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Reserva
    {
        public int ReservaId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        [Required]
        public int HabitacionId { get; set; }
        public Habitacion? Habitacion { get; set; }
        [Required]
        public DateTime FechaEntrada { get; set; }
        [Required]
        public DateTime FechaSalida { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; } = "Pendiente";
    }
}