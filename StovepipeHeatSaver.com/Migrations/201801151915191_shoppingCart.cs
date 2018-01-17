namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shoppingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.String(),
                        Country = c.String(),
                        PayeeEmail = c.String(),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.PromoCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 100),
                        PromotionalItemId = c.Int(),
                        PromotionalItemPrice = c.Decimal(precision: 18, scale: 2),
                        WithPurchaseOfId = c.Int(),
                        MinimumQualifyingPurchase = c.Decimal(precision: 18, scale: 2),
                        PercentOffOrder = c.Decimal(precision: 18, scale: 2),
                        PercentOffItem = c.Decimal(precision: 18, scale: 2),
                        SpecialPrice = c.Decimal(precision: 18, scale: 2),
                        SpecialPriceItemId = c.Int(),
                        CodeStart = c.DateTime(),
                        CodeEnd = c.DateTime(),
                        ShoppingCart_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductBases", t => t.PromotionalItemId)
                .ForeignKey("dbo.ProductBases", t => t.SpecialPriceItemId)
                .ForeignKey("dbo.ProductBases", t => t.WithPurchaseOfId)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.PromotionalItemId)
                .Index(t => t.WithPurchaseOfId)
                .Index(t => t.SpecialPriceItemId)
                .Index(t => t.ShoppingCart_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoCodes", "ShoppingCart_Id", "dbo.ShoppingCarts");
            DropForeignKey("dbo.PromoCodes", "WithPurchaseOfId", "dbo.ProductBases");
            DropForeignKey("dbo.PromoCodes", "SpecialPriceItemId", "dbo.ProductBases");
            DropForeignKey("dbo.PromoCodes", "PromotionalItemId", "dbo.ProductBases");
            DropForeignKey("dbo.ShoppingCarts", "Order_Id", "dbo.Orders");
            DropIndex("dbo.PromoCodes", new[] { "ShoppingCart_Id" });
            DropIndex("dbo.PromoCodes", new[] { "SpecialPriceItemId" });
            DropIndex("dbo.PromoCodes", new[] { "WithPurchaseOfId" });
            DropIndex("dbo.PromoCodes", new[] { "PromotionalItemId" });
            DropIndex("dbo.PromoCodes", new[] { "Code" });
            DropIndex("dbo.ShoppingCarts", new[] { "Order_Id" });
            DropTable("dbo.PromoCodes");
            DropTable("dbo.ShoppingCarts");
        }
    }
}
