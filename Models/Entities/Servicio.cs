using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class Servicio
    {
        public int ServicioId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(200)]
        public string? Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
    }
}