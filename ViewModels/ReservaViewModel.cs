using System.ComponentModel.DataAnnotations;

namespace hotelv1.ViewModels
{
    // Ejemplo de ViewModel para crear o editar una reserva
    public class ReservaViewModel
    {
        [Display(Name = "Método de Pago")]
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente válido.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La habitación es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una habitación válida.")]
        [Display(Name = "Habitación")]
        public int HabitacionId { get; set; }

        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de entrada")]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0.")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        [Display(Name = "Estado")]
        public string? Estado { get; set; } = "Pendiente";

        // Para mostrar listas en el formulario
        public IEnumerable<hotelv1.Models.Entities.Cliente>? Clientes { get; set; }
        public IEnumerable<hotelv1.Models.Entities.Habitacion>? Habitaciones { get; set; }
        public IEnumerable<string> MetodosPago { get; set; } = new List<string> { "Efectivo", "Tarjeta", "Transferencia" };
    }
}
