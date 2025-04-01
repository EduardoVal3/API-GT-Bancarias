namespace GestiondTransaccionesBancarias.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigurarRelaciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CuentaBancarias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titular = c.String(nullable: false),
                        Saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaInteres = c.Decimal(precision: 18, scale: 2),
                        LimiteSobregiro = c.Decimal(precision: 18, scale: 2),
                        RazonSocial = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false),
                        Rol = c.String(nullable: false),
                        SalarioPorHora = c.Decimal(precision: 18, scale: 2),
                        HorasTrabajadas = c.Int(),
                        FechaInicio = c.DateTime(),
                        FechaFin = c.DateTime(),
                        SalarioMensual = c.Decimal(precision: 18, scale: 2),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prestamoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titular = c.String(nullable: false),
                        MontoPrestamo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaInteres = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CuotasPendientes = c.Int(nullable: false),
                        TipoPropiedad = c.String(),
                        Finalidad = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarjetas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumeroTarjeta = c.String(nullable: false),
                        Titular = c.String(nullable: false),
                        LimiteCredito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaldoPendiente = c.Decimal(precision: 18, scale: 2),
                        SaldoDisponible = c.Decimal(precision: 18, scale: 2),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transaccions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha = c.DateTime(nullable: false),
                        CuentaId = c.Int(),
                        PrestamoId = c.Int(),
                        CuentaId1 = c.Int(),
                        CuentaOrigenId = c.Int(),
                        CuentaDestinoId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CuentaBancarias", t => t.CuentaId)
                .ForeignKey("dbo.Prestamoes", t => t.PrestamoId, cascadeDelete: true)
                .ForeignKey("dbo.CuentaBancarias", t => t.CuentaId1)
                .ForeignKey("dbo.CuentaBancarias", t => t.CuentaDestinoId)
                .ForeignKey("dbo.CuentaBancarias", t => t.CuentaOrigenId)
                .Index(t => t.CuentaId)
                .Index(t => t.PrestamoId)
                .Index(t => t.CuentaId1)
                .Index(t => t.CuentaOrigenId)
                .Index(t => t.CuentaDestinoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaccions", "CuentaOrigenId", "dbo.CuentaBancarias");
            DropForeignKey("dbo.Transaccions", "CuentaDestinoId", "dbo.CuentaBancarias");
            DropForeignKey("dbo.Transaccions", "CuentaId1", "dbo.CuentaBancarias");
            DropForeignKey("dbo.Transaccions", "PrestamoId", "dbo.Prestamoes");
            DropForeignKey("dbo.Transaccions", "CuentaId", "dbo.CuentaBancarias");
            DropIndex("dbo.Transaccions", new[] { "CuentaDestinoId" });
            DropIndex("dbo.Transaccions", new[] { "CuentaOrigenId" });
            DropIndex("dbo.Transaccions", new[] { "CuentaId1" });
            DropIndex("dbo.Transaccions", new[] { "PrestamoId" });
            DropIndex("dbo.Transaccions", new[] { "CuentaId" });
            DropTable("dbo.Transaccions");
            DropTable("dbo.Tarjetas");
            DropTable("dbo.Prestamoes");
            DropTable("dbo.Empleadoes");
            DropTable("dbo.CuentaBancarias");
        }
    }
}
