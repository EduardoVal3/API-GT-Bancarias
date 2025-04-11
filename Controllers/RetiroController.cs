using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using GestiondTransaccionesBancarias.Models;

namespace GestiondTransaccionesBancarias.Controllers
{
    /// <summary>
    /// Controlador para gestionar retiros.
    /// </summary>
    [RoutePrefix("api/retiro")]
    public class RetiroController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de retiros.
        /// </summary>
        /// <returns>Lista de retiros.</returns>
        public IQueryable<Retiro> GetRetiros()
        {
            return db.Transacciones.OfType<Retiro>().Include(t => t.Cuenta);
        }

        /// <summary>
        /// Obtiene un retiro por su ID.
        /// </summary>
        /// <param name="id">ID del retiro.</param>
        /// <returns>Retiro encontrado.</returns>
        /// <response code="200">Devuelve el retiro encontrado.</response>
        /// <response code="404">Si el retiro no es encontrado.</response>
        [ResponseType(typeof(Retiro))]
        public async Task<IHttpActionResult> GetRetiro(int id)
        {
            Retiro retiro = await db.Transacciones.OfType<Retiro>().Include(t => t.Cuenta)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (retiro == null)
            {
                return NotFound();
            }

            return Ok(retiro);
        }

        /// <summary>
        /// Actualiza un retiro existente.
        /// </summary>
        /// <param name="id">ID del retiro.</param>
        /// <param name="retiro">Datos del retiro a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si el retiro no es encontrado.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRetiro(int id, Retiro retiro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != retiro.Id)
            {
                return BadRequest("El ID del retiro no coincide con el ID proporcionado.");
            }

            // --- Validación de cuenta de origen ---
            if (!db.CuentasBancarias.Any(c => c.Id == retiro.CuentaId))
            {
                return BadRequest("La cuenta de origen no existe.");
            }

            // --- Validación de saldo suficiente ---
            var cuentaOrigen = await db.CuentasBancarias.FindAsync(retiro.CuentaId);
            if (cuentaOrigen.Saldo < retiro.Monto)
            {
                return BadRequest("Saldo insuficiente en la cuenta de origen.");
            }

            // --- Transacción atómica ---
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Actualizar saldo de la cuenta origen
                    cuentaOrigen.Saldo -= retiro.Monto;

                    db.Entry(retiro).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetiroExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Crea un nuevo retiro.
        /// </summary>
        /// <param name="retiro">Datos del retiro a crear.</param>
        /// <returns>Retiro creado.</returns>
        /// <response code="201">Retiro creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(Retiro))]
        public async Task<IHttpActionResult> PostRetiro(Retiro retiro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- Validación de cuenta de origen ---
            if (!db.CuentasBancarias.Any(c => c.Id == retiro.CuentaId))
            {
                return BadRequest("La cuenta de origen no existe.");
            }

            // --- Validación de saldo suficiente ---
            var cuentaOrigen = await db.CuentasBancarias.FindAsync(retiro.CuentaId);
            if (cuentaOrigen.Saldo < retiro.Monto)
            {
                return BadRequest("Saldo insuficiente en la cuenta de origen.");
            }

            // --- Transacción atómica ---
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Actualizar saldo de la cuenta origen
                    cuentaOrigen.Saldo -= retiro.Monto;

                    db.Transacciones.Add(retiro);
                    await db.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (DbUpdateException)
                {
                    return InternalServerError();
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = retiro.Id }, retiro);
        }

        /// <summary>
        /// Elimina un retiro por su ID.
        /// </summary>
        /// <param name="id">ID del retiro.</param>
        /// <returns>Retiro eliminado.</returns>
        /// <response code="200">Retiro eliminado con éxito.</response>
        /// <response code="404">Si el retiro no es encontrado.</response>
        [ResponseType(typeof(Retiro))]
        public async Task<IHttpActionResult> DeleteRetiro(int id)
        {
            Retiro retiro = await db.Transacciones.OfType<Retiro>().Include(t => t.Cuenta)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (retiro == null)
            {
                return NotFound();
            }

            db.Transacciones.Remove(retiro);
            await db.SaveChangesAsync();

            return Ok(retiro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RetiroExists(int id)
        {
            return db.Transacciones.OfType<Retiro>()
                .Count(r => r.Id == id) > 0;
        }
    }
}