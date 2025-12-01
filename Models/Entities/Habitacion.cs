using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Habitacion
    {
        public int HabitacionId { get; set; }
        [Required]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Tipo { get; set; } = string.Empty; // Usar listado en el ViewModel

        [Required]
        public decimal PrecioPorNoche { get; set; }

        public bool Disponible { get; set; } = true;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [StringLength(200)]
        public string? ObservacionEstado { get; set; }

        public bool Ocupada { get; set; } = false;

        [StringLength(200)]
        public string? FotoUrl { get; set; } // URL o nombre de archivo de la foto
    }
}