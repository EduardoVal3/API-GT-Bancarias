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
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int? EmpleadoId { get; set; } // Null porque no siempre habra un empleado invlucrado
        public Empleado Empleado { get; set; }

        public int CuentaId { get; set; }
        public CuentaBancaria Cuenta { get; set; }
    }
}