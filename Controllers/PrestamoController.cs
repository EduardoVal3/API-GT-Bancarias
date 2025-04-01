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
    /// Controlador para gestionar préstamos (hipotecarios y personales).
    /// </summary>
    [RoutePrefix("api/prestamo")]
    public class PrestamoController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene todos los préstamos.
        /// </summary>
        /// <returns>Lista de préstamos.</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<Prestamo> Get()
        {
            return db.Prestamos.ToList();
        }

        /// <summary>
        /// Obtiene un préstamo por ID.
        /// </summary>
        /// <param name="id">ID del préstamo.</param>
        /// <returns>Préstamo encontrado.</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var prestamo = db.Prestamos.Find(id);
            if (prestamo == null) return NotFound();
            return Ok(prestamo);
        }

        /// <summary>
        /// Crea un nuevo préstamo.
        /// </summary>
        /// <param name="tipoPrestamo">Tipo de préstamo ("Hipotecario" o "Personal").</param>
        /// <param name="prestamo">Datos del préstamo.</param>
        /// <returns>Préstamo creado.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromUri] TipoPrestamo tipoPrestamo, [FromBody] Prestamo prestamo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            switch (tipoPrestamo)
            {
                case TipoPrestamo.Hipotecario:
                    var prestamoHipotecario = new PrestamoHipotecario
                    {
                        MontoPrestamo = prestamo.MontoPrestamo,
                        TasaInteres = prestamo.TasaInteres,
                        CuotasPendientes = prestamo.CuotasPendientes,
                        FechaPago = prestamo.FechaPago,
                        Estado = prestamo.Estado,
                        ClienteId = prestamo.ClienteId,
                        TipoPropiedad = ((dynamic)prestamo).TipoPropiedad
                    };
                    db.Prestamos.Add(prestamoHipotecario);
                    break;

                case TipoPrestamo.Personal:
                    var prestamoPersonal = new PrestamoPersonal
                    {
                        MontoPrestamo = prestamo.MontoPrestamo,
                        TasaInteres = prestamo.TasaInteres,
                        CuotasPendientes = prestamo.CuotasPendientes,
                        FechaPago = prestamo.FechaPago,
                        Estado = prestamo.Estado,
                        ClienteId = prestamo.ClienteId,
                        Finalidad = ((dynamic)prestamo).Finalidad
                    };
                    db.Prestamos.Add(prestamoPersonal);
                    break;

                default:
                    return BadRequest("Tipo de préstamo no válido.");
            }

            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = prestamo.Id }, prestamo);
        }

        /// <summary>
        /// Actualiza un préstamo existente.
        /// </summary>
        /// <param name="id">ID del préstamo.</param>
        /// <param name="prestamo">Datos actualizados del préstamo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Prestamo prestamo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var prestamoExistente = db.Prestamos.Find(id);
            if (prestamoExistente == null) return NotFound();

            prestamoExistente.MontoPrestamo = prestamo.MontoPrestamo;
            prestamoExistente.TasaInteres = prestamo.TasaInteres;
            prestamoExistente.CuotasPendientes = prestamo.CuotasPendientes;
            prestamoExistente.FechaPago = prestamo.FechaPago;
            prestamoExistente.Estado = prestamo.Estado;
            prestamoExistente.ClienteId = prestamo.ClienteId;

            if (prestamoExistente is PrestamoHipotecario prestamoHipotecario)
            {
                prestamoHipotecario.TipoPropiedad = ((dynamic)prestamo).TipoPropiedad;
            }
            else if (prestamoExistente is PrestamoPersonal prestamoPersonal)
            {
                prestamoPersonal.Finalidad = ((dynamic)prestamo).Finalidad;
            }

            db.Entry(prestamoExistente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Elimina un préstamo.
        /// </summary>
        /// <param name="id">ID del préstamo.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var prestamo = db.Prestamos.Find(id);
            if (prestamo == null) return NotFound();

            db.Prestamos.Remove(prestamo);
            db.SaveChanges();
            return Ok();
        }
    }
}