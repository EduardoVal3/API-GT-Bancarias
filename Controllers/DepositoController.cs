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
    /// Controlador para gestionar depósitos.
    /// </summary>
    [RoutePrefix("api/deposito")]
    public class DepositoController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de depósitos.
        /// </summary>
        /// <returns>Lista de depósitos.</returns>
        public IQueryable<Deposito> GetDepositos()
        {
            return db.Transacciones.OfType<Deposito>().Include(t => t.Cuenta);
        }

        /// <summary>
        /// Obtiene un depósito por su ID.
        /// </summary>
        /// <param name="id">ID del depósito.</param>
        /// <returns>Depósito encontrado.</returns>
        /// <response code="200">Devuelve el depósito encontrado.</response>
        /// <response code="404">Si el depósito no es encontrado.</response>
        [ResponseType(typeof(Deposito))]
        public async Task<IHttpActionResult> GetDeposito(int id)
        {
            Deposito deposito = await db.Transacciones.OfType<Deposito>().Include(t => t.Cuenta)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (deposito == null)
            {
                return NotFound();
            }

            return Ok(deposito);
        }

        /// <summary>
        /// Actualiza un depósito existente.
        /// </summary>
        /// <param name="id">ID del depósito.</param>
        /// <param name="deposito">Datos del depósito a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si el depósito no es encontrado.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDeposito(int id, Deposito deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deposito.Id)
            {
                return BadRequest("El ID del depósito no coincide con el ID proporcionado.");
            }

            // --- Validación de cuenta de destino ---
            if (!db.CuentasBancarias.Any(c => c.Id == deposito.CuentaId))
            {
                return BadRequest("La cuenta de destino no existe.");
            }

            // --- Transacción atómica ---
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Actualizar saldo de la cuenta destino
                    var cuentaDestino = await db.CuentasBancarias.FindAsync(deposito.CuentaId);
                    cuentaDestino.Saldo += deposito.Monto;

                    db.Entry(deposito).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepositoExists(id))
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
        /// Crea un nuevo depósito.
        /// </summary>
        /// <param name="deposito">Datos del depósito a crear.</param>
        /// <returns>Depósito creado.</returns>
        /// <response code="201">Depósito creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(Deposito))]
        public async Task<IHttpActionResult> PostDeposito(Deposito deposito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- Validación de cuenta de destino ---
            if (!db.CuentasBancarias.Any(c => c.Id == deposito.CuentaId))
            {
                return BadRequest("La cuenta de destino no existe.");
            }

            // --- Transacción atómica ---
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Actualizar saldo de la cuenta destino
                    var cuentaDestino = await db.CuentasBancarias.FindAsync(deposito.CuentaId);
                    cuentaDestino.Saldo += deposito.Monto;

                    db.Transacciones.Add(deposito);
                    await db.SaveChangesAsync();

                    transaction.Complete();
                }
                catch (DbUpdateException)
                {
                    return InternalServerError();
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = deposito.Id }, deposito);
        }

        /// <summary>
        /// Elimina un depósito por su ID.
        /// </summary>
        /// <param name="id">ID del depósito.</param>
        /// <returns>Depósito eliminado.</returns>
        /// <response code="200">Depósito eliminado con éxito.</response>
        /// <response code="404">Si el depósito no es encontrado.</response>
        [ResponseType(typeof(Deposito))]
        public async Task<IHttpActionResult> DeleteDeposito(int id)
        {
            Deposito deposito = await db.Transacciones.OfType<Deposito>().Include(t => t.Cuenta)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (deposito == null)
            {
                return NotFound();
            }

            db.Transacciones.Remove(deposito);
            await db.SaveChangesAsync();

            return Ok(deposito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepositoExists(int id)
        {
            return db.Transacciones.OfType<Deposito>()
                .Count(d => d.Id == id) > 0;
        }
    }
}