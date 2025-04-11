using GestiondTransaccionesBancarias.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestiondTransaccionesBancarias.Controllers
{
    /// <summary>
    /// Controlador que maneja operaciones relacionadas con transferencias bancarias.
    /// Proporciona endpoints para crear, leer, actualizar y eliminar transferencias.
    /// </summary>
    [RoutePrefix("api/transferencia")]
    public class TransferenciaController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // --- GET ---
        /// <summary>
        /// Obtiene todas las transferencias existentes, incluyendo información de las cuentas origen y destino.
        /// </summary>
        /// <returns>Lista de transferencias con sus relaciones cargadas.</returns>
        [Route("")]
        public IQueryable<Transferencia> GetTransferencias()
        {
            return db.Transacciones.OfType<Transferencia>()
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino);
        }

        /// <summary>
        /// Obtiene una transferencia específica por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la transferencia.</param>
        /// <returns>La transferencia encontrada o un error 404 si no existe.</returns>
        [Route("{id:int}", Name = "GetTransferencia")]
        [ResponseType(typeof(Transferencia))]
        public async Task<IHttpActionResult> GetTransferencia(int id)
        {
            Transferencia transferencia = await db.Transacciones.OfType<Transferencia>()
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transferencia == null)
            {
                return NotFound();
            }

            return Ok(transferencia);
        }

        // --- PUT ---
        /// <summary>
        /// Actualiza una transferencia existente.
        /// </summary>
        /// <param name="id">Identificador de la transferencia a actualizar.</param>
        /// <param name="transferencia">Objeto Transferencia con los cambios.</param>
        /// <returns>Estado HTTP 204 (No Content) si la operación es exitosa.</returns>
        [HttpPut]
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransferencia(int id, Transferencia transferencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transferencia.Id)
            {
                return BadRequest("El ID no coincide con el de la transferencia.");
            }

            var cuentaOrigen = await db.CuentasBancarias.FindAsync(transferencia.CuentaOrigenId);
            var cuentaDestino = await db.CuentasBancarias.FindAsync(transferencia.CuentaDestinoId);

            if (cuentaOrigen == null || cuentaDestino == null)
            {
                return BadRequest("Una de las cuentas no existe.");
            }

            if (transferencia.CuentaId != transferencia.CuentaOrigenId)
            {
                transferencia.CuentaId = transferencia.CuentaOrigenId;
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    cuentaOrigen.Saldo -= transferencia.Monto;
                    cuentaDestino.Saldo += transferencia.Monto;
                    db.Entry(transferencia).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    transaction.Complete();
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbEntityValidationException dbEx)
            {
                var validationErrors = dbEx.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                return InternalServerError(new Exception($"Validación fallida: {string.Join(", ", validationErrors)}"));
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? "Sin detalles";
                return InternalServerError(new Exception($"Error: {ex.Message}. Detalles: {innerException}"));
            }
        }

        // --- POST ---
        /// <summary>
        /// Crea una nueva transferencia.
        /// </summary>
        /// <param name="transferencia">Objeto Transferencia con los datos de la nueva transferencia.</param>
        /// <returns>La transferencia creada con su identificador asignado.</returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Transferencia))]
        public async Task<IHttpActionResult> PostTransferencia(Transferencia transferencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (transferencia.CuentaOrigenId == transferencia.CuentaDestinoId)
            {
                return BadRequest("No se permite transferir a la misma cuenta.");
            }

            var cuentaOrigen = await db.CuentasBancarias.FindAsync(transferencia.CuentaOrigenId);
            var cuentaDestino = await db.CuentasBancarias.FindAsync(transferencia.CuentaDestinoId);

            if (cuentaOrigen == null || cuentaDestino == null)
            {
                return BadRequest("Una de las cuentas no existe.");
            }

            if (cuentaOrigen.Saldo < transferencia.Monto)
            {
                return BadRequest("Saldo insuficiente en la cuenta de origen.");
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    transferencia.CuentaId = transferencia.CuentaOrigenId;
                    cuentaOrigen.Saldo -= transferencia.Monto;
                    cuentaDestino.Saldo += transferencia.Monto;
                    db.Transacciones.Add(transferencia);
                    await db.SaveChangesAsync();
                    transaction.Complete();
                }

                return CreatedAtRoute("GetTransferencia", new { id = transferencia.Id }, transferencia);
            }
            catch (DbEntityValidationException dbEx)
            {
                var validationErrors = dbEx.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                return InternalServerError(new Exception($"Validación fallida: {string.Join(", ", validationErrors)}"));
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? "Sin detalles";
                return InternalServerError(new Exception($"Error: {ex.Message}. Detalles: {innerException}"));
            }
        }

        // --- DELETE ---
        /// <summary>
        /// Elimina una transferencia y reversa los cambios en los saldos de las cuentas.
        /// </summary>
        /// <param name="id">Identificador de la transferencia a eliminar.</param>
        /// <returns>La transferencia eliminada o un error 404 si no existe.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [ResponseType(typeof(Transferencia))]
        public async Task<IHttpActionResult> DeleteTransferencia(int id)
        {
            Transferencia transferencia = await db.Transacciones.OfType<Transferencia>()
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transferencia == null)
            {
                return NotFound();
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var cuentaOrigen = await db.CuentasBancarias.FindAsync(transferencia.CuentaOrigenId);
                    var cuentaDestino = await db.CuentasBancarias.FindAsync(transferencia.CuentaDestinoId);

                    if (cuentaOrigen != null && cuentaDestino != null)
                    {
                        cuentaOrigen.Saldo += transferencia.Monto;
                        cuentaDestino.Saldo -= transferencia.Monto;
                    }

                    db.Transacciones.Remove(transferencia);
                    await db.SaveChangesAsync();
                    transaction.Complete();
                }

                return Ok(transferencia);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? "Sin detalles";
                return InternalServerError(new Exception($"Error al eliminar: {ex.Message}. Detalles: {innerException}"));
            }
        }

        // --- Métodos auxiliares ---
        /// <summary>
        /// Libera los recursos asociados al contexto de la base de datos.
        /// </summary>
        /// <param name="disposing">Indica si se está realizando una disposición explícita.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Verifica si una transferencia con el identificador especificado existe.
        /// </summary>
        /// <param name="id">Identificador de la transferencia.</param>
        /// <returns>True si la transferencia existe, de lo contrario False.</returns>
        private bool TransferenciaExists(int id)
        {
            return db.Transacciones.OfType<Transferencia>().Count(e => e.Id == id) > 0;
        }
    }
}