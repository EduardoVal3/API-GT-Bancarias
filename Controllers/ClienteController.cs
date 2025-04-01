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
    /// Controlador para gestionar clientes.
    /// </summary>
    [RoutePrefix("api/cliente")]
    public class ClienteController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Cliente> Get()
        {
            return db.Clientes.ToList();
        }

        /// <summary>
        /// Obtiene un cliente por ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente encontrado.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="cliente">Datos del cliente.</param>
        /// <returns>Cliente creado.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            db.Clientes.Add(cliente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <param name="cliente">Datos actualizados del cliente.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var clienteExistente = db.Clientes.Find(id);
            if (clienteExistente == null) return NotFound();

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Direccion = cliente.Direccion;

            db.Entry(clienteExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null) return NotFound();

            db.Clientes.Remove(cliente);
            db.SaveChanges();
            return Ok();
        }
    }
}