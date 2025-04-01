using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public enum TipoTarjeta
    {
        Credito,
        Debito
    }
    public class Tarjeta : EntityBase
    {
        public string NumeroTarjeta { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string CVV { get; set; }

        // Relaciones
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}