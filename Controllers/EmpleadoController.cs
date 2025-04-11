using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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
        /// Obtiene la lista de empleados.
        /// </summary>
        /// <returns>Lista de empleados.</returns>
        public IQueryable<Empleado> GetEmpleados()
        {
            return db.Personas.OfType<Empleado>();
        }

        /// <summary>
        /// Obtiene un empleado por su ID.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>Empleado encontrado.</returns>
        /// <response code="200">Devuelve el empleado encontrado.</response>
        /// <response code="404">Si el empleado no es encontrado.</response>
        [ResponseType(typeof(Empleado))]
        public async Task<IHttpActionResult> GetEmpleado(int id)
        {
            Empleado empleado = await db.Personas.OfType<Empleado>().FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return Ok(empleado);
        }

        /// <summary>
        /// Actualiza un empleado existente.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <param name="empleado">Datos del empleado a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si el empleado no es encontrado.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmpleado(int id, Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empleado.Id)
            {
                return BadRequest();
            }

            db.Entry(empleado).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Crea un nuevo empleado.
        /// </summary>
        /// <param name="empleado">Datos del empleado a crear.</param>
        /// <returns>Empleado creado.</returns>
        /// <response code="201">Empleado creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(Empleado))]
        public async Task<IHttpActionResult> PostEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Personas.Add(empleado);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = empleado.Id }, empleado);
        }

        /// <summary>
        /// Elimina un empleado por su ID.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>Empleado eliminado.</returns>
        /// <response code="200">Empleado eliminado con éxito.</response>
        /// <response code="404">Si el empleado no es encontrado.</response>
        [ResponseType(typeof(Empleado))]
        public async Task<IHttpActionResult> DeleteEmpleado(int id)
        {
            Empleado empleado = await db.Personas.OfType<Empleado>().FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            db.Personas.Remove(empleado);
            await db.SaveChangesAsync();

            return Ok(empleado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpleadoExists(int id)
        {
            return db.Personas.OfType<Empleado>().Count(e => e.Id == id) > 0;
        }
    }
}