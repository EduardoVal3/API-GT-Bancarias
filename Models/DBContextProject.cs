using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GestiondTransaccionesBancarias.Models
{
    public class DBContextProject : DbContext
    {
        public DBContextProject() : base("MyDbConnectionString") { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        // Configuración adicional del modelo
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para herencia: Tabla por jerarquía (TPH) para Tarjetas
            modelBuilder.Entity<Tarjeta>()
                        .Map<TarjetaCredito>(m => m.Requires("TipoTarjeta").HasValue("Credito"))
                        .Map<TarjetaDebito>(m => m.Requires("TipoTarjeta").HasValue("Debito"));

            // Configuración para herencia: Tabla por jerarquía (TPH) para Préstamos
            modelBuilder.Entity<Prestamo>()
                        .Map<PrestamoHipotecario>(m => m.Requires("TipoPrestamo").HasValue("Hipotecario"))
                        .Map<PrestamoPersonal>(m => m.Requires("TipoPrestamo").HasValue("Personal"));

            // Configuración para herencia: Tabla por jerarquía (TPH) para Transacciones
            modelBuilder.Entity<Transaccion>()
                        .Map<Transferencia>(m => m.Requires("TipoTransaccion").HasValue("Transferencia"));

            // Configuración de relaciones
            modelBuilder.Entity<Cliente>()
                        .HasMany(c => c.CuentasBancarias)
                        .WithRequired(cb => cb.Cliente)
                        .HasForeignKey(cb => cb.ClienteId);

            modelBuilder.Entity<Cliente>()
                        .HasMany(c => c.Prestamos)
                        .WithRequired(p => p.Cliente)
                        .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<Cliente>()
                        .HasMany(c => c.Tarjetas)
                        .WithRequired(t => t.Cliente)
                        .HasForeignKey(t => t.ClienteId);

            modelBuilder.Entity<Cliente>()
                        .HasMany(c => c.Transacciones)
                        .WithRequired(t => t.Cliente)
                        .HasForeignKey(t => t.ClienteId);

            modelBuilder.Entity<Empleado>()
                        .HasMany(e => e.Transacciones)
                        .WithOptional(t => t.Empleado)
                        .HasForeignKey(t => t.EmpleadoId);

            modelBuilder.Entity<CuentaBancaria>()
                        .HasMany(cb => cb.Transacciones)
                        .WithRequired(t => t.Cuenta)
                        .HasForeignKey(t => t.CuentaId);

            // Configuración para la relación CuentaId
            modelBuilder.Entity<Transaccion>()
                        .HasRequired(t => t.Cuenta)
                        .WithMany()
                        .HasForeignKey(t => t.CuentaId)
                        .WillCascadeOnDelete(false); // Desactivar cascada para esta relación

            // Configuración para la relación CuentaOrigen
            modelBuilder.Entity<Transferencia>()
                .HasRequired(t => t.CuentaOrigen)
                .WithMany()
                .HasForeignKey(t => t.CuentaOrigenId)
                .WillCascadeOnDelete(false); // Desactivar cascada para esta relación

    // Configuración para la relación CuentaDestino
    modelBuilder.Entity<Transferencia>()
                .HasRequired(t => t.CuentaDestino)
                .WithMany()
                .HasForeignKey(t => t.CuentaDestinoId)
                .WillCascadeOnDelete(false); // Desactivar cascada para esta relación
        }
    }
}