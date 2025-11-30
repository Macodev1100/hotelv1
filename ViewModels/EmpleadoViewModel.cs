using System.ComponentModel.DataAnnotations;

namespace hotelv1.ViewModels
{
    public class EmpleadoViewModel
    {
        public int EmpleadoId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Puesto")]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener exactamente 9 dígitos y solo números.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [Display(Name = "Fecha de contratación")]
        public DateTime FechaContratacion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;
    }
}