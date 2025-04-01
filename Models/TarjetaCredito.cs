using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    /// <summary>
    /// Representa una tarjeta de crédito.
    /// </summary>
    public class TarjetaCredito : Tarjeta
    {
        /// <summary>
        /// Límite de crédito disponible.
        /// </summary>
        public decimal LimiteCredito { get; set; }
        /// <summary>
        /// Saldo pendiente a pagar.
        /// </summary>
        public decimal SaldoPendiente { get; set; }
    }
}