using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Factura
    {
        public int FacturaId { get; set; }
        [Required]
        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal MontoTotal { get; set; }
        [StringLength(200)]
        public string? Detalle { get; set; }
    }
}