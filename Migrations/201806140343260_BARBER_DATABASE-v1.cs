namespace BarberSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BARBER_DATABASEv1 : DbMigration
    {
        public override void Up()
        {
           // AddColumn("dbo.CLIENTES", "status_cliente", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
           // DropColumn("dbo.CLIENTES", "status_cliente");
        }
    }
}
