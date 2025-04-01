namespace GestiondTransaccionesBancarias.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixForeignKeyConstraints : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transaccions", "PrestamoId", "dbo.Prestamoes");
            DropForeignKey("dbo.Transaccions", "CuentaId1", "dbo.CuentaBancarias");
            DropIndex("dbo.Transaccions", new[] { "CuentaId" });
            DropIndex("dbo.Transaccions", new[] { "PrestamoId" });
            DropIndex("dbo.Transaccions", new[] { "CuentaId1" });
            RenameColumn(table: "dbo.Transaccions", name: "Discriminator", newName: "TipoTransaccion");
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Email = c.String(),
                        Telefono = c.String(),
                        Direccion = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CuentaBancarias", "NumeroCuenta", c => c.String());
            AddColumn("dbo.CuentaBancarias", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.CuentaBancarias", "ClienteId", c => c.Int(nullable: false));
            AddColumn("dbo.CuentaBancarias", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.CuentaBancarias", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Empleadoes", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Empleadoes", "Nombre", c => c.String());
            AddColumn("dbo.Empleadoes", "Apellido", c => c.String());
            AddColumn("dbo.Empleadoes", "Email", c => c.String());
            AddColumn("dbo.Empleadoes", "Telefono", c => c.String());
            AddColumn("dbo.Empleadoes", "Direccion", c => c.String());
            AddColumn("dbo.Empleadoes", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Empleadoes", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prestamoes", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Prestamoes", "FechaPago", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prestamoes", "Estado", c => c.Int(nullable: false));
            AddColumn("dbo.Prestamoes", "ClienteId", c => c.Int(nullable: false));
            AddColumn("dbo.Prestamoes", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prestamoes", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prestamoes", "TipoPrestamo", c => c.String(maxLength: 128));
            AddColumn("dbo.Tarjetas", "FechaExpiracion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tarjetas", "CVV", c => c.String());
            AddColumn("dbo.Tarjetas", "ClienteId", c => c.Int(nullable: false));
            AddColumn("dbo.Tarjetas", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tarjetas", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tarjetas", "TipoTarjeta", c => c.String(maxLength: 128));
            AddColumn("dbo.Transaccions", "Descripcion", c => c.String());
            AddColumn("dbo.Transaccions", "ClienteId", c => c.Int(nullable: false));
            AddColumn("dbo.Transaccions", "EmpleadoId", c => c.Int());
            AddColumn("dbo.Transaccions", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Transaccions", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tarjetas", "NumeroTarjeta", c => c.String());
            AlterColumn("dbo.Tarjetas", "LimiteCredito", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Transaccions", "Tipo", c => c.Int(nullable: false));
            AlterColumn("dbo.Transaccions", "CuentaId", c => c.Int(nullable: false));
            AlterColumn("dbo.Transaccions", "TipoTransaccion", c => c.String(maxLength: 128));
            CreateIndex("dbo.CuentaBancarias", "ClienteId");
            CreateIndex("dbo.Transaccions", "ClienteId");
            CreateIndex("dbo.Transaccions", "EmpleadoId");
            CreateIndex("dbo.Transaccions", "CuentaId");
            CreateIndex("dbo.Prestamoes", "ClienteId");
            CreateIndex("dbo.Tarjetas", "ClienteId");
            AddForeignKey("dbo.Transaccions", "EmpleadoId", "dbo.Empleadoes", "Id");
            AddForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Clientes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Clientes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Clientes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transaccions", "ClienteId", "dbo.Clientes", "Id", cascadeDelete: true);
            DropColumn("dbo.CuentaBancarias", "Titular");
            DropColumn("dbo.CuentaBancarias", "TasaInteres");
            DropColumn("dbo.CuentaBancarias", "LimiteSobregiro");
            DropColumn("dbo.CuentaBancarias", "RazonSocial");
            DropColumn("dbo.CuentaBancarias", "Discriminator");
            DropColumn("dbo.Empleadoes", "NombreCompleto");
            DropColumn("dbo.Empleadoes", "Rol");
            DropColumn("dbo.Empleadoes", "SalarioPorHora");
            DropColumn("dbo.Empleadoes", "HorasTrabajadas");
            DropColumn("dbo.Empleadoes", "FechaInicio");
            DropColumn("dbo.Empleadoes", "FechaFin");
            DropColumn("dbo.Empleadoes", "SalarioMensual");
            DropColumn("dbo.Empleadoes", "Discriminator");
            DropColumn("dbo.Prestamoes", "Titular");
            DropColumn("dbo.Prestamoes", "Discriminator");
            DropColumn("dbo.Tarjetas", "Titular");
            DropColumn("dbo.Tarjetas", "Discriminator");
            DropColumn("dbo.Transaccions", "PrestamoId");
            DropColumn("dbo.Transaccions", "CuentaId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaccions", "CuentaId1", c => c.Int());
            AddColumn("dbo.Transaccions", "PrestamoId", c => c.Int());
            AddColumn("dbo.Tarjetas", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Tarjetas", "Titular", c => c.String(nullable: false));
            AddColumn("dbo.Prestamoes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Prestamoes", "Titular", c => c.String(nullable: false));
            AddColumn("dbo.Empleadoes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Empleadoes", "SalarioMensual", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Empleadoes", "FechaFin", c => c.DateTime());
            AddColumn("dbo.Empleadoes", "FechaInicio", c => c.DateTime());
            AddColumn("dbo.Empleadoes", "HorasTrabajadas", c => c.Int());
            AddColumn("dbo.Empleadoes", "SalarioPorHora", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Empleadoes", "Rol", c => c.String(nullable: false));
            AddColumn("dbo.Empleadoes", "NombreCompleto", c => c.String(nullable: false));
            AddColumn("dbo.CuentaBancarias", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CuentaBancarias", "RazonSocial", c => c.String());
            AddColumn("dbo.CuentaBancarias", "LimiteSobregiro", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CuentaBancarias", "TasaInteres", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CuentaBancarias", "Titular", c => c.String(nullable: false));
            DropForeignKey("dbo.Transaccions", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Clientes");
            DropForeignKey("dbo.Transaccions", "EmpleadoId", "dbo.Empleadoes");
            DropIndex("dbo.Tarjetas", new[] { "ClienteId" });
            DropIndex("dbo.Prestamoes", new[] { "ClienteId" });
            DropIndex("dbo.Transaccions", new[] { "CuentaId" });
            DropIndex("dbo.Transaccions", new[] { "EmpleadoId" });
            DropIndex("dbo.Transaccions", new[] { "ClienteId" });
            DropIndex("dbo.CuentaBancarias", new[] { "ClienteId" });
            AlterColumn("dbo.Transaccions", "TipoTransaccion", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Transaccions", "CuentaId", c => c.Int());
            AlterColumn("dbo.Transaccions", "Tipo", c => c.String(nullable: false));
            AlterColumn("dbo.Tarjetas", "LimiteCredito", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Tarjetas", "NumeroTarjeta", c => c.String(nullable: false));
            DropColumn("dbo.Transaccions", "UpdatedAt");
            DropColumn("dbo.Transaccions", "CreatedAt");
            DropColumn("dbo.Transaccions", "EmpleadoId");
            DropColumn("dbo.Transaccions", "ClienteId");
            DropColumn("dbo.Transaccions", "Descripcion");
            DropColumn("dbo.Tarjetas", "TipoTarjeta");
            DropColumn("dbo.Tarjetas", "UpdatedAt");
            DropColumn("dbo.Tarjetas", "CreatedAt");
            DropColumn("dbo.Tarjetas", "ClienteId");
            DropColumn("dbo.Tarjetas", "CVV");
            DropColumn("dbo.Tarjetas", "FechaExpiracion");
            DropColumn("dbo.Prestamoes", "TipoPrestamo");
            DropColumn("dbo.Prestamoes", "UpdatedAt");
            DropColumn("dbo.Prestamoes", "CreatedAt");
            DropColumn("dbo.Prestamoes", "ClienteId");
            DropColumn("dbo.Prestamoes", "Estado");
            DropColumn("dbo.Prestamoes", "FechaPago");
            DropColumn("dbo.Prestamoes", "Tipo");
            DropColumn("dbo.Empleadoes", "UpdatedAt");
            DropColumn("dbo.Empleadoes", "CreatedAt");
            DropColumn("dbo.Empleadoes", "Direccion");
            DropColumn("dbo.Empleadoes", "Telefono");
            DropColumn("dbo.Empleadoes", "Email");
            DropColumn("dbo.Empleadoes", "Apellido");
            DropColumn("dbo.Empleadoes", "Nombre");
            DropColumn("dbo.Empleadoes", "Tipo");
            DropColumn("dbo.CuentaBancarias", "UpdatedAt");
            DropColumn("dbo.CuentaBancarias", "CreatedAt");
            DropColumn("dbo.CuentaBancarias", "ClienteId");
            DropColumn("dbo.CuentaBancarias", "Tipo");
            DropColumn("dbo.CuentaBancarias", "NumeroCuenta");
            DropTable("dbo.Clientes");
            RenameColumn(table: "dbo.Transaccions", name: "TipoTransaccion", newName: "Discriminator");
            CreateIndex("dbo.Transaccions", "CuentaId1");
            CreateIndex("dbo.Transaccions", "PrestamoId");
            CreateIndex("dbo.Transaccions", "CuentaId");
            AddForeignKey("dbo.Transaccions", "CuentaId1", "dbo.CuentaBancarias", "Id");
            AddForeignKey("dbo.Transaccions", "PrestamoId", "dbo.Prestamoes", "Id", cascadeDelete: true);
        }
    }
}
