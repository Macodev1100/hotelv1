using System.ComponentModel.DataAnnotations;

namespace hotelv1.Models.Entities
{
    public class ReservaServicio
    {
        [Key]
        public int ReservaServicioId { get; set; }
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }
    }
}