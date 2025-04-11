namespace GestiondTransaccionesBancarias.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTPHEnfoque : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Transaccions", name: "TipoTransaccion", newName: "Discriminator");
            AddColumn("dbo.Prestamoes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Tarjetas", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Transaccions", "HechoPor", c => c.String());
            AlterColumn("dbo.Transaccions", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Prestamoes", "Tipo");
            DropColumn("dbo.Prestamoes", "TipoPrestamo");
            DropColumn("dbo.Tarjetas", "Tipo");
            DropColumn("dbo.Tarjetas", "TipoTarjeta");
            DropColumn("dbo.Transaccions", "Tipo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaccions", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Tarjetas", "TipoTarjeta", c => c.String(maxLength: 128));
            AddColumn("dbo.Tarjetas", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Prestamoes", "TipoPrestamo", c => c.String(maxLength: 128));
            AddColumn("dbo.Prestamoes", "Tipo", c => c.Int(nullable: false));
            AlterColumn("dbo.Transaccions", "Discriminator", c => c.String(maxLength: 128));
            DropColumn("dbo.Transaccions", "HechoPor");
            DropColumn("dbo.Tarjetas", "Discriminator");
            DropColumn("dbo.Prestamoes", "Discriminator");
            RenameColumn(table: "dbo.Transaccions", name: "Discriminator", newName: "TipoTransaccion");
        }
    }
}
