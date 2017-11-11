namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 30),
                        Person = c.String(maxLength: 30),
                        Date = c.DateTime(),
                        Location = c.String(),
                        Text = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Reviews", new[] { "Date" });
            DropTable("dbo.Reviews");
        }
    }
}
