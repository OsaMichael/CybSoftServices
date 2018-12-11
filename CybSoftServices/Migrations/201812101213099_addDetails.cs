namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Access_Details", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Access_Details");
        }
    }
}
