using GestiondTransaccionesBancarias.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestiondTransaccionesBancarias.Controllers
{
    /// <summary>
    /// Controlador para gestionar tarjetas de crédito.
    /// </summary>
    [RoutePrefix("api/tarjeta/credito")]
    public class TarjetaCreditoController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de tarjetas de crédito.
        /// </summary>
        /// <returns>Lista de tarjetas de crédito.</returns>
        public IQueryable<TarjetaCredito> GetTarjetasCredito()
        {
            return db.Tarjetas.OfType<TarjetaCredito>().Include(t => t.Cliente);
        }

        /// <summary>
        /// Obtiene una tarjeta de crédito por su ID.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Tarjeta de crédito encontrada.</returns>
        /// <response code="200">Devuelve la tarjeta encontrada.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> GetTarjetaCredito(int id)
        {
            TarjetaCredito tarjeta = await db.Tarjetas.OfType<TarjetaCredito>().Include(c => c.Cliente).FirstOrDefaultAsync(t => t.Id == id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            return Ok(tarjeta);
        }

        /// <summary>
        /// Actualiza una tarjeta de crédito existente.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <param name="tarjeta">Datos de la tarjeta a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarjetaCredito(int id, TarjetaCredito tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjeta.Id)
            {
                return BadRequest("El ID de la tarjeta no coincide con el ID proporcionado.");
            }

            // --- Validación de cliente ---
            if (!db.Personas.Any(p => p.Id == tarjeta.ClienteId))
            {
                return BadRequest("El cliente asociado no existe.");
            }

            // --- Validación de número de tarjeta único ---
            if (await db.Tarjetas.AnyAsync(t => t.NumeroTarjeta == tarjeta.NumeroTarjeta && t.Id != id))
            {
                return BadRequest("El número de tarjeta ya está en uso.");
            }

            db.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaCreditoExists(id))
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
        /// Crea una nueva tarjeta de crédito.
        /// </summary>
        /// <param name="tarjeta">Datos de la tarjeta a crear.</param>
        /// <returns>Tarjeta de crédito creada.</returns>
        /// <response code="201">Tarjeta creada con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> PostTarjetaCredito(TarjetaCredito tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- Validación de cliente ---
            if (!db.Personas.Any(p => p.Id == tarjeta.ClienteId))
            {
                return BadRequest("El cliente asociado no existe.");
            }

            // --- Validación de número de tarjeta único ---
            if (await db.Tarjetas.AnyAsync(t => t.NumeroTarjeta == tarjeta.NumeroTarjeta))
            {
                return BadRequest("El número de tarjeta ya está en uso.");
            }

            db.Tarjetas.Add(tarjeta);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tarjeta.Id }, tarjeta);
        }

        /// <summary>
        /// Elimina una tarjeta de crédito por su ID.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Tarjeta eliminada.</returns>
        /// <response code="200">Tarjeta eliminada con éxito.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(TarjetaCredito))]
        public async Task<IHttpActionResult> DeleteTarjetaCredito(int id)
        {
            TarjetaCredito tarjeta = await db.Tarjetas.OfType<TarjetaCredito>().Include(t => t.Cliente).FirstOrDefaultAsync(t => t.Id == id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            db.Tarjetas.Remove(tarjeta);
            await db.SaveChangesAsync();

            return Ok(tarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaCreditoExists(int id)
        {
            return db.Tarjetas.OfType<TarjetaCredito>()
                .Count(t => t.Id == id) > 0;
        }
    }
}