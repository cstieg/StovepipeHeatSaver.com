namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Cart", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Cart");
        }
    }
}
