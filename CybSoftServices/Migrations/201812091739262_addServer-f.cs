namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addServerf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        ServerID = c.Int(nullable: false, identity: true),
                        Discription = c.String(),
                        Services = c.String(),
                        QTY = c.Int(nullable: false),
                        Charge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpiringDate = c.DateTime(nullable: false),
                        RAM = c.String(),
                        HardDisk = c.String(),
                        HDD_Used = c.String(),
                        HDD_Available = c.String(),
                        Access_Details = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Servers");
        }
    }
}
