using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public enum TipoTransaccion
    {
        Deposito,
        Retiro,
        Transferencia
    }

    public class Transaccion : EntityBase
    {
        public decimal Monto { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        // Relaciones
        public int CuentaId { get; set; }
        public CuentaBancaria Cuenta { get; set; }
    }
}