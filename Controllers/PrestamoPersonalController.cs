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
    /// Controlador para gestionar préstamos personales.
    /// </summary>
    [RoutePrefix("api/prestamopersonal")]
    public class PrestamoPersonalController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de préstamos personales.
        /// </summary>
        /// <returns>Lista de préstamos personales.</returns>
        public IQueryable<PrestamoPersonal> GetPrestamosPersonales()
        {
            return db.Prestamos.OfType<PrestamoPersonal>()
            .Include(t => t.Cliente);
        }

        /// <summary>
        /// Obtiene un préstamo personal por su ID.
        /// </summary>
        /// <param name="id">ID del préstamo personal.</param>
        /// <returns>Préstamo personal encontrado.</returns>
        /// <response code="200">Devuelve el préstamo encontrado.</response>
        /// <response code="404">Si el préstamo no es encontrado.</response>
        [ResponseType(typeof(PrestamoPersonal))]
        public async Task<IHttpActionResult> GetPrestamoPersonal(int id)
        {
            PrestamoPersonal prestamo = await db.Prestamos.OfType<PrestamoPersonal>().Include(t => t.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return Ok(prestamo);
        }

        /// <summary>
        /// Actualiza un préstamo personal existente.
        /// </summary>
        /// <param name="id">ID del préstamo.</param>
        /// <param name="prestamo">Datos del préstamo a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si el préstamo no es encontrado.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPrestamoPersonal(int id, PrestamoPersonal prestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prestamo.Id)
            {
                return BadRequest();
            }

            db.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoPersonalExists(id))
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
        /// Crea un nuevo préstamo personal.
        /// </summary>
        /// <param name="prestamo">Datos del préstamo a crear.</param>
        /// <returns>Préstamo creado.</returns>
        /// <response code="201">Préstamo creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(PrestamoPersonal))]
        public async Task<IHttpActionResult> PostPrestamoPersonal(PrestamoPersonal prestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prestamos.Add(prestamo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = prestamo.Id }, prestamo);
        }

        /// <summary>
        /// Elimina un préstamo personal por su ID.
        /// </summary>
        /// <param name="id">ID del préstamo.</param>
        /// <returns>Préstamo eliminado.</returns>
        /// <response code="200">Préstamo eliminado con éxito.</response>
        /// <response code="404">Si el préstamo no es encontrado.</response>
        [ResponseType(typeof(PrestamoPersonal))]
        public async Task<IHttpActionResult> DeletePrestamoPersonal(int id)
        {
            PrestamoPersonal prestamo = await db.Prestamos.OfType<PrestamoPersonal>()
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            db.Prestamos.Remove(prestamo);
            await db.SaveChangesAsync();

            return Ok(prestamo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrestamoPersonalExists(int id)
        {
            return db.Prestamos.OfType<PrestamoPersonal>()
                .Count(p => p.Id == id) > 0;
        }
    }
}