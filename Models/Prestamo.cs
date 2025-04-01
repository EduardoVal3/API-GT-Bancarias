using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public enum TipoPrestamo
    {
        Hipotecario,
        Personal
    }
    public enum EstadoActual
    {
        Activo,
        Pagado,
        Vencido
    }
    public class Prestamo : EntityBase
    {
        public TipoPrestamo Tipo { get; set; }
        public decimal MontoPrestamo { get; set; }
        public decimal TasaInteres { get; set; }
        public int CuotasPendientes { get; set; }
        public DateTime FechaPago { get; set; }
        public EstadoActual Estado { get; set; } // "Activo", "Pagado", "Vencido"

        // Relaciones
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}