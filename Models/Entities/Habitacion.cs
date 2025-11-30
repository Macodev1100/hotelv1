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
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;
        [Required]
        public decimal PrecioPorNoche { get; set; }
        public bool Disponible { get; set; } = true;
        [StringLength(200)]
        public string? Descripcion { get; set; }
    }
}