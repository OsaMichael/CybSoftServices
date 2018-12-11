namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDetailsF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ServerDescription", c => c.String());
            AddColumn("dbo.Services", "Services", c => c.String(nullable: false));
            AddColumn("dbo.Services", "ExpiringDate", c => c.String(nullable: false));
            DropColumn("dbo.Services", "Name");
            DropColumn("dbo.Services", "Description");
            DropColumn("dbo.Services", "ExpiredDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "ExpiredDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Services", "Description", c => c.String());
            AddColumn("dbo.Services", "Name", c => c.String());
            DropColumn("dbo.Services", "ExpiringDate");
            DropColumn("dbo.Services", "Services");
            DropColumn("dbo.Services", "ServerDescription");
        }
    }
}
