using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
	public abstract class Persona : EntityBase
	{
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}