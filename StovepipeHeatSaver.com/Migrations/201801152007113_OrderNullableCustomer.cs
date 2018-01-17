namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderNullableCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "BillToAddressId", "dbo.ShipToAddresses");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "ShipToAddressId", "dbo.ShipToAddresses");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Orders", new[] { "ShipToAddressId" });
            DropIndex("dbo.Orders", new[] { "BillToAddressId" });
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int());
            AlterColumn("dbo.Orders", "ShipToAddressId", c => c.Int());
            AlterColumn("dbo.Orders", "BillToAddressId", c => c.Int());
            CreateIndex("dbo.Orders", "CustomerId");
            CreateIndex("dbo.Orders", "ShipToAddressId");
            CreateIndex("dbo.Orders", "BillToAddressId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "BillToAddressId" });
            DropIndex("dbo.Orders", new[] { "ShipToAddressId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            AlterColumn("dbo.Orders", "BillToAddressId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "ShipToAddressId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "BillToAddressId");
            CreateIndex("dbo.Orders", "ShipToAddressId");
            CreateIndex("dbo.Orders", "CustomerId");
        }
    }
}
