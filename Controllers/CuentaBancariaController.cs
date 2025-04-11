using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GestiondTransaccionesBancarias.Models;

namespace GestiondTransaccionesBancarias.Controllers
{
    /// <summary>
    /// Controlador para gestionar cuentas bancarias.
    /// </summary>
    [RoutePrefix("api/cuentabancaria")]
    public class CuentaBancariaController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de cuentas bancarias.
        /// </summary>
        /// <returns>Lista de cuentas bancarias.</returns>
        public IQueryable<CuentaBancaria> GetCuentasBancarias()
        {
            return db.CuentasBancarias
                .Include(t => t.Cliente);
        }

        /// <summary>
        /// Obtiene una cuenta bancaria por su ID.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <returns>Cuenta bancaria encontrada.</returns>
        /// <response code="200">Devuelve la cuenta bancaria encontrada.</response>
        /// <response code="404">Si la cuenta bancaria no es encontrada.</response>
        [ResponseType(typeof(CuentaBancaria))]
        public async Task<IHttpActionResult> GetCuentaBancaria(int id)
        {
            CuentaBancaria cuentaBancaria = await db.CuentasBancarias
                .Include(t => t.Cliente)
                .Include(c => c.Transacciones)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (cuentaBancaria == null)
            {
                return NotFound();
            }

            return Ok(cuentaBancaria);
        }
        
        /// <summary>
        /// Actualiza una cuenta bancaria existente.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <param name="cuentaBancaria">Datos de la cuenta bancaria a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si la cuenta bancaria no es encontrada.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCuentaBancaria(int id, CuentaBancaria cuentaBancaria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuentaBancaria.Id)
            {
                return BadRequest();
            }

            // Verificar que el ClienteId exista en la base de datos
            if (!db.Personas.OfType<Cliente>().Any(c => c.Id == cuentaBancaria.ClienteId))
            {
                return BadRequest("El ClienteId especificado no existe.");
            }

            db.Entry(cuentaBancaria).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaBancariaExists(id))
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
        /// Crea una nueva cuenta bancaria.
        /// </summary>
        /// <param name="cuentaBancaria">Datos de la cuenta bancaria a crear.</param>
        /// <returns>Cuenta bancaria creada.</returns>
        /// <response code="201">Cuenta bancaria creada con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(CuentaBancaria))]
        public async Task<IHttpActionResult> PostCuentaBancaria(CuentaBancaria cuentaBancaria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el ClienteId exista en la base de datos
            if (!db.Personas.OfType<Cliente>().Any(c => c.Id == cuentaBancaria.ClienteId))
            {
                return BadRequest("El ClienteId especificado no existe.");
            }

            // Opcional: Inicializar propiedades adicionales si no se proporcionan
            if (cuentaBancaria.Transacciones == null)
            {
                cuentaBancaria.Transacciones = new List<Transaccion>();
            }

            db.CuentasBancarias.Add(cuentaBancaria);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuentaBancaria.Id }, cuentaBancaria);
        }

        /// <summary>
        /// Elimina una cuenta bancaria por su ID.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <returns>Cuenta bancaria eliminada.</returns>
        /// <response code="200">Cuenta bancaria eliminada con éxito.</response>
        /// <response code="404">Si la cuenta bancaria no es encontrada.</response>
        [ResponseType(typeof(CuentaBancaria))]
        public async Task<IHttpActionResult> DeleteCuentaBancaria(int id)
        {
            CuentaBancaria cuentaBancaria = await db.CuentasBancarias.FindAsync(id);
            if (cuentaBancaria == null)
            {
                return NotFound();
            }

            // Opcional: Eliminar las transacciones antes de eliminar la cuenta bancaria
            db.Transacciones.RemoveRange(cuentaBancaria.Transacciones);

            db.CuentasBancarias.Remove(cuentaBancaria);
            await db.SaveChangesAsync();

            return Ok(cuentaBancaria);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CuentaBancariaExists(int id)
        {
            return db.CuentasBancarias.Count(e => e.Id == id) > 0;
        }
    }
}