namespace StovepipeHeatSaver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductExtension : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ShipToAddresses", newName: "Addresses");
            RenameTable(name: "dbo.ProductBases", newName: "Products");
            DropForeignKey("dbo.PromoCodes", "ShoppingCart_Id", "dbo.ShoppingCarts");
            DropForeignKey("dbo.ShoppingCarts", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "Cart" });
            DropIndex("dbo.ShoppingCarts", new[] { "Order_Id" });
            DropIndex("dbo.PromoCodes", new[] { "ShoppingCart_Id" });
            RenameColumn(table: "dbo.ShoppingCarts", name: "Order_Id", newName: "OrderId");
            CreateTable(
                "dbo.ProductExtensions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Diameter = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PromoCodeAddeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShoppingCartId = c.Int(nullable: false),
                        PromoCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PromoCodes", t => t.PromoCodeId, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.ShoppingCartId)
                .Index(t => t.PromoCodeId);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BaseUrl = c.String(maxLength: 100),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 1000),
                        Country = c.String(maxLength: 2),
                        Currency = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderDetails", "Tax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Tax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "NoteToPayee", c => c.String(maxLength: 255));
            AddColumn("dbo.Products", "Sku", c => c.String(maxLength: 20));
            AddColumn("dbo.Products", "Gtin", c => c.String(maxLength: 14));
            AddColumn("dbo.Products", "Brand", c => c.String(maxLength: 70));
            AddColumn("dbo.Products", "UrlName", c => c.String(maxLength: 20));
            AddColumn("dbo.Products", "MetaDescription", c => c.String(maxLength: 250));
            AddColumn("dbo.Products", "Condition", c => c.String(maxLength: 15));
            AddColumn("dbo.Products", "GoogleProductCategory", c => c.String(maxLength: 500));
            AddColumn("dbo.Products", "ProductExtensionId", c => c.Int());
            AddColumn("dbo.ShippingCountries", "BaseShippingIsPerItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShippingCountries", "AdditionalShippingIsPerItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShippingCountries", "FreeShipping", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShoppingCarts", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "EmailAddress", c => c.String(maxLength: 254));
            AlterColumn("dbo.Orders", "Cart", c => c.String(maxLength: 30));
            AlterColumn("dbo.ShoppingCarts", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "EmailAddress", unique: true);
            CreateIndex("dbo.Countries", "IsoCode2", unique: true);
            CreateIndex("dbo.Orders", "Cart");
            CreateIndex("dbo.Products", "Sku");
            CreateIndex("dbo.Products", "UrlName");
            CreateIndex("dbo.ShoppingCarts", "OrderId");
            AddForeignKey("dbo.ShoppingCarts", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            DropColumn("dbo.Products", "Diameter");
            DropColumn("dbo.Products", "Discriminator");
            DropColumn("dbo.ShoppingCarts", "PayeeEmail");
            DropColumn("dbo.PromoCodes", "ShoppingCart_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoCodes", "ShoppingCart_Id", c => c.Int());
            AddColumn("dbo.ShoppingCarts", "PayeeEmail", c => c.String());
            AddColumn("dbo.Products", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Products", "Diameter", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.ShoppingCarts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.PromoCodeAddeds", "ShoppingCartId", "dbo.ShoppingCarts");
            DropForeignKey("dbo.PromoCodeAddeds", "PromoCodeId", "dbo.PromoCodes");
            DropForeignKey("dbo.ProductExtensions", "ProductId", "dbo.Products");
            DropIndex("dbo.ShoppingCarts", new[] { "OrderId" });
            DropIndex("dbo.PromoCodeAddeds", new[] { "PromoCodeId" });
            DropIndex("dbo.PromoCodeAddeds", new[] { "ShoppingCartId" });
            DropIndex("dbo.ProductExtensions", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "UrlName" });
            DropIndex("dbo.Products", new[] { "Sku" });
            DropIndex("dbo.Orders", new[] { "Cart" });
            DropIndex("dbo.Countries", new[] { "IsoCode2" });
            DropIndex("dbo.Customers", new[] { "EmailAddress" });
            AlterColumn("dbo.ShoppingCarts", "OrderId", c => c.Int());
            AlterColumn("dbo.Orders", "Cart", c => c.String(maxLength: 20));
            AlterColumn("dbo.Customers", "EmailAddress", c => c.String());
            DropColumn("dbo.ShoppingCarts", "Created");
            DropColumn("dbo.ShippingCountries", "FreeShipping");
            DropColumn("dbo.ShippingCountries", "AdditionalShippingIsPerItem");
            DropColumn("dbo.ShippingCountries", "BaseShippingIsPerItem");
            DropColumn("dbo.Products", "ProductExtensionId");
            DropColumn("dbo.Products", "GoogleProductCategory");
            DropColumn("dbo.Products", "Condition");
            DropColumn("dbo.Products", "MetaDescription");
            DropColumn("dbo.Products", "UrlName");
            DropColumn("dbo.Products", "Brand");
            DropColumn("dbo.Products", "Gtin");
            DropColumn("dbo.Products", "Sku");
            DropColumn("dbo.Orders", "NoteToPayee");
            DropColumn("dbo.Orders", "Tax");
            DropColumn("dbo.Orders", "Created");
            DropColumn("dbo.OrderDetails", "Tax");
            DropTable("dbo.Stores");
            DropTable("dbo.PromoCodeAddeds");
            DropTable("dbo.ProductExtensions");
            RenameColumn(table: "dbo.ShoppingCarts", name: "OrderId", newName: "Order_Id");
            CreateIndex("dbo.PromoCodes", "ShoppingCart_Id");
            CreateIndex("dbo.ShoppingCarts", "Order_Id");
            CreateIndex("dbo.Orders", "Cart");
            AddForeignKey("dbo.ShoppingCarts", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.PromoCodes", "ShoppingCart_Id", "dbo.ShoppingCarts", "Id");
            RenameTable(name: "dbo.Products", newName: "ProductBases");
            RenameTable(name: "dbo.Addresses", newName: "ShipToAddresses");
        }
    }
}
