namespace GestiondTransaccionesBancarias.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarTablasYRelaciones : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clientes", newName: "Personas");
            AddColumn("dbo.Personas", "Tipo", c => c.Int());
            AddColumn("dbo.Personas", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Personas", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Personas", "Apellido", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Personas", "Email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Personas", "Telefono", c => c.String(maxLength: 20));
            AlterColumn("dbo.Personas", "Direccion", c => c.String(maxLength: 255));
            AlterColumn("dbo.CuentaBancarias", "NumeroCuenta", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Transaccions", "Descripcion", c => c.String(maxLength: 255));
            AlterColumn("dbo.Tarjetas", "NumeroTarjeta", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Tarjetas", "CVV", c => c.String(nullable: false, maxLength: 4));
            CreateIndex("dbo.CuentaBancarias", "NumeroCuenta", unique: true);
            CreateIndex("dbo.Tarjetas", "NumeroTarjeta", unique: true);
            DropTable("dbo.Empleadoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.Int(nullable: false),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Email = c.String(),
                        Telefono = c.String(),
                        Direccion = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Tarjetas", new[] { "NumeroTarjeta" });
            DropIndex("dbo.CuentaBancarias", new[] { "NumeroCuenta" });
            AlterColumn("dbo.Tarjetas", "CVV", c => c.String());
            AlterColumn("dbo.Tarjetas", "NumeroTarjeta", c => c.String());
            AlterColumn("dbo.Transaccions", "Descripcion", c => c.String());
            AlterColumn("dbo.CuentaBancarias", "NumeroCuenta", c => c.String());
            AlterColumn("dbo.Personas", "Direccion", c => c.String());
            AlterColumn("dbo.Personas", "Telefono", c => c.String());
            AlterColumn("dbo.Personas", "Email", c => c.String());
            AlterColumn("dbo.Personas", "Apellido", c => c.String());
            AlterColumn("dbo.Personas", "Nombre", c => c.String());
            DropColumn("dbo.Personas", "Discriminator");
            DropColumn("dbo.Personas", "Tipo");
            RenameTable(name: "dbo.Personas", newName: "Clientes");
        }
    }
}
