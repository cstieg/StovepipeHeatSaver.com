namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "FirstName", c => c.String());
            AddColumn("dbo.Customers", "LastName", c => c.String());
            AlterColumn("dbo.Customers", "CustomerName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "CustomerName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Customers", "LastName");
            DropColumn("dbo.Customers", "FirstName");
        }
    }
}
