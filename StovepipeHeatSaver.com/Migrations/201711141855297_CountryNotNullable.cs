namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Countries", "IsoCode2", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Countries", "IsoCode2", c => c.String(maxLength: 2));
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 50));
        }
    }
}
