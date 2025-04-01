using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public class TarjetaDebito : Tarjeta
    {
        public decimal SaldoDisponible { get; set; }
    }
}