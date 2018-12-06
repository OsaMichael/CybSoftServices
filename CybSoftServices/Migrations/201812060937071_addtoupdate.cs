namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtoupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "UserId");
        }
    }
}
