using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    /// <summary>
    /// Representa un préstamo personal.
    /// </summary>
    public class PrestamoPersonal : Prestamo
    {
        /// <summary>
        /// Finalidad del préstamo personal.
        /// </summary>
        public string Finalidad { get; set; }
    }
}