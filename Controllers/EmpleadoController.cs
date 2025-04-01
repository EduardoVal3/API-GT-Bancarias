using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using GestiondTransaccionesBancarias.Models;

namespace GestiondTransaccionesBancarias.Controllers
{
    /// <summary>
    /// Controlador para gestionar empleados.
    /// </summary>
    [RoutePrefix("api/empleado")]
    public class EmpleadoController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todos los empleados.
        /// </summary>
        /// <returns>Lista de empleados.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Empleado> Get()
        {
            return db.Empleados.ToList();
        }

        /// <summary>
        /// Obtiene un empleado por ID.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>Empleado encontrado.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var empleado = db.Empleados.Find(id);
            if (empleado == null) return NotFound();
            return Ok(empleado);
        }

        /// <summary>
        /// Crea un nuevo empleado.
        /// </summary>
        /// <param name="empleado">Datos del empleado.</param>
        /// <returns>Empleado creado.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Empleado empleado)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            db.Empleados.Add(empleado);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empleado.Id }, empleado);
        }

        /// <summary>
        /// Actualiza un empleado existente.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <param name="empleado">Datos actualizados del empleado.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Empleado empleado)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var empleadoExistente = db.Empleados.Find(id);
            if (empleadoExistente == null) return NotFound();

            empleadoExistente.Nombre = empleado.Nombre;
            empleadoExistente.Apellido = empleado.Apellido;
            empleadoExistente.Email = empleado.Email;
            empleadoExistente.Telefono = empleado.Telefono;
            empleadoExistente.Direccion = empleado.Direccion;
            empleadoExistente.Tipo = empleado.Tipo;

            db.Entry(empleadoExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina un empleado.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var empleado = db.Empleados.Find(id);
            if (empleado == null) return NotFound();

            db.Empleados.Remove(empleado);
            db.SaveChanges();
            return Ok();
        }
    }
}