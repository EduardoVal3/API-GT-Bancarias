using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public enum TipoEmpleado
    {
        Cajero,
        Gerente,
        Asistente,
        Auditor
    }
    public class Empleado : Persona
    {
        public TipoEmpleado Tipo { get; set; }
    }
}