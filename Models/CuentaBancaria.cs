using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestiondTransaccionesBancarias.Models
{
    public enum TipoCuenta
    {
        Ahorro,
        Corriente,
        Empresarial
    }
    public class CuentaBancaria : EntityBase
    {
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }

        public TipoCuenta Tipo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}