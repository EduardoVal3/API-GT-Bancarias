using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public class Transferencia : Transaccion
    {
        public int CuentaOrigenId { get; set; }

        public virtual CuentaBancaria CuentaOrigen { get; set; }

        public int CuentaDestinoId { get; set; }

        public virtual CuentaBancaria CuentaDestino { get; set; }
    }
}