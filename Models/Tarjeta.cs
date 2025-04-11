using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestiondTransaccionesBancarias.Models
{
    public abstract class Tarjeta : EntityBase
    {
        public string NumeroTarjeta { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string CVV { get; set; }

        // Relaciones
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}