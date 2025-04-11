using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Propiedad de sombra para manejar el nombre del enum
        [NotMapped]
        public string TipoString
        {
            get => Tipo.ToString();
            set => Tipo = (TipoCuenta)Enum.Parse(typeof(TipoCuenta), value, true);
        }

        // Propiedad original para el enum
        public TipoCuenta Tipo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}