using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public string TipoString
        {
            get => Tipo.ToString();
            set => Tipo = (TipoEmpleado)Enum.Parse(typeof(TipoEmpleado), value, true);
        }
        public TipoEmpleado Tipo { get; set; }
    }
}