namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderCascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Orders", "ShipToAddressId", "dbo.ShipToAddresses", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "BillToAddressId", "dbo.ShipToAddresses", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
        }
    }
}
