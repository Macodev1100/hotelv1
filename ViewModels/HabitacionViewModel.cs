using System.ComponentModel.DataAnnotations;

namespace hotelv1.ViewModels
{
    public class HabitacionViewModel
    {
        public int HabitacionId { get; set; }


        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [RegularExpression(@"^\d{1,4}$", ErrorMessage = "El número debe ser solo números y máximo 4 dígitos.")]
        [Display(Name = "Número")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El tipo no puede superar los 30 caracteres.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio por noche es obligatorio.")]
        [Range(0, 100000, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        [Display(Name = "Precio por noche")]
        public decimal PrecioPorNoche { get; set; }

        [Display(Name = "Disponible")]
        public bool Disponible { get; set; } = true;

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}