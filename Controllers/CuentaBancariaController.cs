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
    /// Controlador para gestionar cuentas bancarias.
    /// </summary>
    [RoutePrefix("api/cuentabancaria")]
    public class CuentaBancariaController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todas las cuentas bancarias.
        /// </summary>
        /// <returns>Lista de cuentas bancarias.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<CuentaBancaria> Get()
        {
            return db.CuentasBancarias.ToList();
        }

        /// <summary>
        /// Obtiene una cuenta bancaria por ID.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <returns>Cuenta bancaria encontrada.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var cuenta = db.CuentasBancarias.Find(id);
            if (cuenta == null) return NotFound();
            return Ok(cuenta);
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria.
        /// </summary>
        /// <param name="cuenta">Datos de la cuenta bancaria.</param>
        /// <returns>Cuenta bancaria creada.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] CuentaBancaria cuenta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            db.CuentasBancarias.Add(cuenta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cuenta.Id }, cuenta);
        }

        /// <summary>
        /// Actualiza una cuenta bancaria existente.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <param name="cuenta">Datos actualizados de la cuenta bancaria.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] CuentaBancaria cuenta)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cuentaExistente = db.CuentasBancarias.Find(id);
            if (cuentaExistente == null) return NotFound();

            cuentaExistente.NumeroCuenta = cuenta.NumeroCuenta;
            cuentaExistente.Saldo = cuenta.Saldo;
            cuentaExistente.ClienteId = cuenta.ClienteId;

            db.Entry(cuentaExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina una cuenta bancaria.
        /// </summary>
        /// <param name="id">ID de la cuenta bancaria.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var cuenta = db.CuentasBancarias.Find(id);
            if (cuenta == null) return NotFound();

            db.CuentasBancarias.Remove(cuenta);
            db.SaveChanges();
            return Ok();
        }
    }
}