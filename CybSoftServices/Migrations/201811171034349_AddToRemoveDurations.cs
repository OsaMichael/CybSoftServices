namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToRemoveDurations : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Duration", c => c.Int(nullable: false));
        }
    }
}
