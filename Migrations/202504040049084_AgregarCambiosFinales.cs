namespace GestiondTransaccionesBancarias.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarCambiosFinales : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Personas");
            AddColumn("dbo.Tarjetas", "Tipo", c => c.Int(nullable: false));
            AddForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Personas", "Id");
            AddForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Personas", "Id");
            AddForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Personas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Personas");
            DropColumn("dbo.Tarjetas", "Tipo");
            AddForeignKey("dbo.Tarjetas", "ClienteId", "dbo.Personas", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prestamoes", "ClienteId", "dbo.Personas", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CuentaBancarias", "ClienteId", "dbo.Personas", "Id", cascadeDelete: true);
        }
    }
}
