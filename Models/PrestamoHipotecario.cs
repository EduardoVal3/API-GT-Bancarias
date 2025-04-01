using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    /// <summary>
    /// Representa un préstamo hipotecario.
    /// </summary>
    public class PrestamoHipotecario : Prestamo
    {
        /// <summary>
        /// Tipo de propiedad asociada al préstamo hipotecario.
        /// </summary>
        public string TipoPropiedad { get; set; }
    }
}