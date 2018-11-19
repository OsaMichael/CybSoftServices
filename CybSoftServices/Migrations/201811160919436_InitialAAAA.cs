namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAAAA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "RenewerType", c => c.String());
            DropColumn("dbo.Services", "RenewedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "RenewedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Services", "RenewerType");
        }
    }
}
