using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
	public class Cliente : Persona
	{
        // Relaciones
        public ICollection<CuentaBancaria> CuentasBancarias { get; set; } = new List<CuentaBancaria>();
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}