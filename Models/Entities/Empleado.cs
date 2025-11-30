using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Puesto { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        public DateTime FechaContratacion { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
    }
}