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
    /// Controlador para gestionar transacciones (depósitos, retiros y transferencias).
    /// </summary>
    [RoutePrefix("api/transaccion")]
    public class TransaccionController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todas las transacciones.
        /// </summary>
        /// <returns>Lista de transacciones.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Transaccion> Get()
        {
            return db.Transacciones.ToList();
        }

        /// <summary>
        /// Obtiene una transacción por ID.
        /// </summary>
        /// <param name="id">ID de la transacción.</param>
        /// <returns>Transacción encontrada.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var transaccion = db.Transacciones.Find(id);
            if (transaccion == null) return NotFound();
            return Ok(transaccion);
        }

        /// <summary>
        /// Crea una nueva transacción.
        /// </summary>
        /// <param name="tipoTransaccion">Tipo de transacción ("Deposito", "Retiro" o "Transferencia").</param>
        /// <param name="transaccion">Datos de la transacción.</param>
        /// <returns>Transacción creada.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromUri] TipoTransaccion tipoTransaccion, [FromBody] Transaccion transaccion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            switch (tipoTransaccion)
            {
                case TipoTransaccion.Transferencia:
                    var transferencia = new Transferencia
                    {
                        Monto = transaccion.Monto,
                        Tipo = transaccion.Tipo,
                        Descripcion = transaccion.Descripcion,
                        Fecha = transaccion.Fecha,
                        ClienteId = transaccion.ClienteId,
                        EmpleadoId = transaccion.EmpleadoId,
                        CuentaId = transaccion.CuentaId,
                        CuentaOrigenId = ((dynamic)transaccion).CuentaOrigenId,
                        CuentaDestinoId = ((dynamic)transaccion).CuentaDestinoId
                    };
                    db.Transacciones.Add(transferencia);
                    break;

                default:
                    db.Transacciones.Add(transaccion);
                    break;
            }

            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = transaccion.Id }, transaccion);
        }

        /// <summary>
        /// Actualiza una transacción existente.
        /// </summary>
        /// <param name="id">ID de la transacción.</param>
        /// <param name="transaccion">Datos actualizados de la transacción.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Transaccion transaccion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaccionExistente = db.Transacciones.Find(id);
            if (transaccionExistente == null) return NotFound();

            transaccionExistente.Monto = transaccion.Monto;
            transaccionExistente.Tipo = transaccion.Tipo;
            transaccionExistente.Descripcion = transaccion.Descripcion;
            transaccionExistente.Fecha = transaccion.Fecha;
            transaccionExistente.ClienteId = transaccion.ClienteId;
            transaccionExistente.EmpleadoId = transaccion.EmpleadoId;
            transaccionExistente.CuentaId = transaccion.CuentaId;

            if (transaccionExistente is Transferencia transferencia)
            {
                transferencia.CuentaOrigenId = ((dynamic)transaccion).CuentaOrigenId;
                transferencia.CuentaDestinoId = ((dynamic)transaccion).CuentaDestinoId;
            }

            db.Entry(transaccionExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina una transacción.
        /// </summary>
        /// <param name="id">ID de la transacción.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var transaccion = db.Transacciones.Find(id);
            if (transaccion == null) return NotFound();

            db.Transacciones.Remove(transaccion);
            db.SaveChanges();
            return Ok();
        }
    }
}