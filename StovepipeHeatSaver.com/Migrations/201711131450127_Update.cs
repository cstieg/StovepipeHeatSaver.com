namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ShippingCountries", new[] { "Country" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ShippingCountries", "Country");
        }
    }
}
