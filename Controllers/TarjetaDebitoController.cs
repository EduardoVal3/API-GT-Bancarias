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
    /// Controlador para gestionar tarjetas de débito.
    /// </summary>
    [RoutePrefix("api/tarjeta/debito")]
    public class TarjetaDebitoController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de tarjetas de débito.
        /// </summary>
        /// <returns>Lista de tarjetas de débito.</returns>
        public IQueryable<TarjetaDebito> GetTarjetasDebito()
        {
            return db.Tarjetas.OfType<TarjetaDebito>().Include(t => t.Cliente);
        }

        /// <summary>
        /// Obtiene una tarjeta de débito por su ID.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Tarjeta de débito encontrada.</returns>
        /// <response code="200">Devuelve la tarjeta encontrada.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(TarjetaDebito))]
        public async Task<IHttpActionResult> GetTarjetaDebito(int id)
        {
            TarjetaDebito tarjeta = await db.Tarjetas.OfType<TarjetaDebito>().Include(c => c.Cliente)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            return Ok(tarjeta);
        }

        /// <summary>
        /// Actualiza una tarjeta de débito existente.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <param name="tarjeta">Datos de la tarjeta a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarjetaDebito(int id, TarjetaDebito tarjeta)
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
                if (!TarjetaDebitoExists(id))
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
        /// Crea una nueva tarjeta de débito.
        /// </summary>
        /// <param name="tarjeta">Datos de la tarjeta a crear.</param>
        /// <returns>Tarjeta de débito creada.</returns>
        /// <response code="201">Tarjeta creada con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(TarjetaDebito))]
        public async Task<IHttpActionResult> PostTarjetaDebito(TarjetaDebito tarjeta)
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
        /// Elimina una tarjeta de débito por su ID.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Tarjeta eliminada.</returns>
        /// <response code="200">Tarjeta eliminada con éxito.</response>
        /// <response code="404">Si la tarjeta no es encontrada.</response>
        [ResponseType(typeof(TarjetaDebito))]
        public async Task<IHttpActionResult> DeleteTarjetaDebito(int id)
        {
            TarjetaDebito tarjeta = await db.Tarjetas.OfType<TarjetaDebito>().Include(c => c.Cliente)
                .FirstOrDefaultAsync(t => t.Id == id);
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

        private bool TarjetaDebitoExists(int id)
        {
            return db.Tarjetas.OfType<TarjetaDebito>()
                .Count(t => t.Id == id) > 0;
        }
    }
}