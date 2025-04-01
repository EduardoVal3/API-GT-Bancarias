using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
	public abstract class EntityBase
	{
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}