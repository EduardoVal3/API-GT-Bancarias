using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestiondTransaccionesBancarias.Models
{
    public abstract class Transaccion : EntityBase
    {
        public string HechoPor { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Relaciones
        public int CuentaId { get; set; }
        public CuentaBancaria Cuenta { get; set; }
    }
}