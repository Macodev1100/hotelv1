using System.ComponentModel.DataAnnotations;

namespace hotelv1.ViewModels
{
    public class FacturaViewModel
    {
        // Para el dropdown de reservas
        public IEnumerable<hotelv1.Models.Entities.Reserva>? Reservas { get; set; }

        public int FacturaId { get; set; }

        [Required(ErrorMessage = "La reserva es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una reserva válida.")]
        [Display(Name = "Reserva")]
        public int ReservaId { get; set; }

        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        [Display(Name = "Fecha de emisión")]
        [DataType(DataType.Date)]
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(0, 1000000, ErrorMessage = "El monto debe ser mayor o igual a 0.")]
        [Display(Name = "Monto total")]
        public decimal MontoTotal { get; set; }

        [StringLength(200, ErrorMessage = "El detalle no puede superar los 200 caracteres.")]
        [Display(Name = "Detalle")]
        public string? Detalle { get; set; }
    }
}