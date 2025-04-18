using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestiondTransaccionesBancarias.Models
{
    public enum EstadoActual
    {
        Activo,
        Pagado,
        Vencido
    }

    public class Prestamo : EntityBase
    {
        public decimal MontoPrestamo { get; set; }
        public decimal TasaInteres { get; set; }
        public int CuotasPendientes { get; set; }
        public DateTime FechaPago { get; set; }
        // Propiedad de sombra para manejar el nombre del enum
        [NotMapped]
        public string TipoString
        {
            get => Estado.ToString();
            set => Estado = (EstadoActual)Enum.Parse(typeof(EstadoActual), value, true);
        }
        public EstadoActual Estado { get; set; } // "Activo", "Pagado", "Vencido"

        // Relaciones
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}