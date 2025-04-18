using System.Data.Entity;
using GestiondTransaccionesBancarias.Models;

namespace GestiondTransaccionesBancarias.Models
{
    public class DBContextProject : DbContext
    {
        public DBContextProject() : base("MyDbConnectionString") { }

        // DbSets para entidades base (incluyen subclases)
        public DbSet<Persona> Personas { get; set; }
        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de propiedades comunes
            ConfigureCommonProperties(modelBuilder);

            // Configuración de relaciones
            ConfigureRelationships(modelBuilder);

            // Configuración de índices únicos
            ConfigureUniqueIndexes(modelBuilder);
        }

        private void ConfigureCommonProperties(DbModelBuilder modelBuilder)
        {
            // Propiedades de Persona
            modelBuilder.Entity<Persona>()
                .Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Persona>()
                .Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Persona>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Persona>()
                .Property(p => p.Telefono)
                .HasMaxLength(20);

            modelBuilder.Entity<Persona>()
                .Property(p => p.Direccion)
                .HasMaxLength(255);

            // Propiedades de CuentaBancaria
            modelBuilder.Entity<CuentaBancaria>()
                .Property(cb => cb.NumeroCuenta)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<CuentaBancaria>()
                .Property(cb => cb.Tipo)
                .HasColumnName("Tipo");

            // Propiedades de Tarjeta
            modelBuilder.Entity<Tarjeta>()
                .Property(t => t.NumeroTarjeta)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Tarjeta>()
                .Property(t => t.CVV)
                .IsRequired()
                .HasMaxLength(4);

            // Propiedades de Transacción
            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto)
                .IsRequired();

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Descripcion)
                .HasMaxLength(255);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Fecha)
                .IsRequired();

            // Propiedad TipoEmpleado en Empleado
            modelBuilder.Entity<Empleado>()
                .Property(e => e.Tipo)
                .HasColumnName("Tipo");

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.Estado)
                .HasColumnName("Estado");
        }

        private void ConfigureRelationships(DbModelBuilder modelBuilder)
        {
            // Relaciones Cliente-CuentaBancaria
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.CuentasBancarias)
                .WithRequired(cb => cb.Cliente)
                .HasForeignKey(cb => cb.ClienteId)
                .WillCascadeOnDelete(false);

            // Relaciones Cliente-Prestamo
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Prestamos)
                .WithRequired(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .WillCascadeOnDelete(false);

            // Relaciones Cliente-Tarjeta
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Tarjetas)
                .WithRequired(t => t.Cliente)
                .HasForeignKey(t => t.ClienteId)
                .WillCascadeOnDelete(false);

            // Relaciones CuentaBancaria-Transaccion
            modelBuilder.Entity<CuentaBancaria>()
                .HasMany(cb => cb.Transacciones)
                .WithRequired(t => t.Cuenta)
                .HasForeignKey(t => t.CuentaId)
                .WillCascadeOnDelete(false);

            // Relaciones específicas de Transferencia
            modelBuilder.Entity<Transferencia>()
                .HasRequired(t => t.CuentaOrigen)
                .WithMany()
                .HasForeignKey(t => t.CuentaOrigenId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transferencia>()
                .HasRequired(t => t.CuentaDestino)
                .WithMany()
                .HasForeignKey(t => t.CuentaDestinoId)
                .WillCascadeOnDelete(false);
        }

        private void ConfigureUniqueIndexes(DbModelBuilder modelBuilder)
        {
            // Índice único para números de cuenta
            modelBuilder.Entity<CuentaBancaria>()
                .HasIndex(cb => cb.NumeroCuenta)
                .IsUnique();

            // Índice único para números de tarjeta
            modelBuilder.Entity<Tarjeta>()
                .HasIndex(t => t.NumeroTarjeta)
                .IsUnique();
        }
    }
}