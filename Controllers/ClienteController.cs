using System.Collections.Generic;
using System.Data;
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
    public class ClienteController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene la lista de clientes.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        public IQueryable<Cliente> GetClientes()
        {
            return db.Personas.OfType<Cliente>();
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente encontrado.</returns>
        /// <response code="200">Devuelve el cliente encontrado.</response>
        /// <response code="404">Si el cliente no es encontrado.</response>
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> GetCliente(int id)
        {
            Cliente cliente = await db.Personas.OfType<Cliente>().Include(c => c.CuentasBancarias).Include(c => c.Prestamos).Include(c => c.Tarjetas).FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <param name="cliente">Datos del cliente a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Operación exitosa sin contenido.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="404">Si el cliente no es encontrado.</response>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cliente.Id)
            {
                return BadRequest();
            }

            db.Entry(cliente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear.</param>
        /// <returns>Cliente creado.</returns>
        /// <response code="201">Cliente creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> PostCliente(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Asegurarse de que las colecciones estén inicializadas si no se proporcionan
            if (cliente.CuentasBancarias == null)
            {
                cliente.CuentasBancarias = new List<CuentaBancaria>();
            }
            if (cliente.Prestamos == null)
            {
                cliente.Prestamos = new List<Prestamo>();
            }
            if (cliente.Tarjetas == null)
            {
                cliente.Tarjetas = new List<Tarjeta>();
            }

            db.Personas.Add(cliente);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente eliminado.</returns>
        /// <response code="200">Cliente eliminado con éxito.</response>
        /// <response code="404">Si el cliente no es encontrado.</response>
        [ResponseType(typeof(Cliente))]
        public async Task<IHttpActionResult> DeleteCliente(int id)
        {
            Cliente cliente = await db.Personas.OfType<Cliente>().Include(c => c.CuentasBancarias).Include(c => c.Prestamos).Include(c => c.Tarjetas).FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            // Eliminar las relaciones antes de eliminar el cliente
            db.CuentasBancarias.RemoveRange(cliente.CuentasBancarias);
            db.Prestamos.RemoveRange(cliente.Prestamos);
            db.Tarjetas.RemoveRange(cliente.Tarjetas);

            db.Personas.Remove(cliente);
            await db.SaveChangesAsync();

            return Ok(cliente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteExists(int id)
        {
            return db.Personas.OfType<Cliente>().Count(e => e.Id == id) > 0;
        }
    }
}