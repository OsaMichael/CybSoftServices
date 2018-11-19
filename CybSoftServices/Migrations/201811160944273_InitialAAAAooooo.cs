namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAAAAooooo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Duration");
        }
    }
}
