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
    /// Controlador para gestionar tarjetas (crédito y débito).
    /// </summary>
    [RoutePrefix("api/tarjeta")]
    public class TarjetaController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todas las tarjetas.
        /// </summary>
        /// <returns>Lista de tarjetas.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Tarjeta> Get()
        {
            return db.Tarjetas.ToList();
        }

        /// <summary>
        /// Obtiene una tarjeta por ID.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Tarjeta encontrada.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var tarjeta = db.Tarjetas.Find(id);
            if (tarjeta == null) return NotFound();
            return Ok(tarjeta);
        }

        /// <summary>
        /// Crea una nueva tarjeta.
        /// </summary>
        /// <param name="tipoTarjeta">Tipo de tarjeta ("Credito" o "Debito").</param>
        /// <param name="tarjeta">Datos de la tarjeta.</param>
        /// <returns>Tarjeta creada.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromUri] TipoTarjeta tipoTarjeta, [FromBody] Tarjeta tarjeta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            switch (tipoTarjeta)
            {
                case TipoTarjeta.Credito:
                    var tarjetaCredito = new TarjetaCredito
                    {
                        NumeroTarjeta = tarjeta.NumeroTarjeta,
                        FechaExpiracion = tarjeta.FechaExpiracion,
                        CVV = tarjeta.CVV,
                        ClienteId = tarjeta.ClienteId,
                        LimiteCredito = ((dynamic)tarjeta).LimiteCredito,
                        SaldoPendiente = ((dynamic)tarjeta).SaldoPendiente
                    };
                    db.Tarjetas.Add(tarjetaCredito);
                    break;

                case TipoTarjeta.Debito:
                    var tarjetaDebito = new TarjetaDebito
                    {
                        NumeroTarjeta = tarjeta.NumeroTarjeta,
                        FechaExpiracion = tarjeta.FechaExpiracion,
                        CVV = tarjeta.CVV,
                        ClienteId = tarjeta.ClienteId,
                        SaldoDisponible = ((dynamic)tarjeta).SaldoDisponible
                    };
                    db.Tarjetas.Add(tarjetaDebito);
                    break;

                default:
                    return BadRequest("Tipo de tarjeta no válido.");
            }

            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = tarjeta.Id }, tarjeta);
        }

        /// <summary>
        /// Actualiza una tarjeta existente.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <param name="tarjeta">Datos actualizados de la tarjeta.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Tarjeta tarjeta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tarjetaExistente = db.Tarjetas.Find(id);
            if (tarjetaExistente == null) return NotFound();

            tarjetaExistente.NumeroTarjeta = tarjeta.NumeroTarjeta;
            tarjetaExistente.FechaExpiracion = tarjeta.FechaExpiracion;
            tarjetaExistente.CVV = tarjeta.CVV;
            tarjetaExistente.ClienteId = tarjeta.ClienteId;

            if (tarjetaExistente is TarjetaCredito tarjetaCredito)
            {
                tarjetaCredito.LimiteCredito = ((dynamic)tarjeta).LimiteCredito;
                tarjetaCredito.SaldoPendiente = ((dynamic)tarjeta).SaldoPendiente;
            }
            else if (tarjetaExistente is TarjetaDebito tarjetaDebito)
            {
                tarjetaDebito.SaldoDisponible = ((dynamic)tarjeta).SaldoDisponible;
            }

            db.Entry(tarjetaExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina una tarjeta.
        /// </summary>
        /// <param name="id">ID de la tarjeta.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var tarjeta = db.Tarjetas.Find(id);
            if (tarjeta == null) return NotFound();

            db.Tarjetas.Remove(tarjeta);
            db.SaveChanges();
            return Ok();
        }
    }
}