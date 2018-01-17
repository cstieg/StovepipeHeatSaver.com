namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DateOrdered", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DateOrdered", c => c.DateTime(nullable: false));
        }
    }
}
