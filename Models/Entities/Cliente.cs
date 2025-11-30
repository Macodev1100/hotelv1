using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        // public ICollection<Reserva>? Reservas { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        [StringLength(200)]
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
    }
}