namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToChangeBoolToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "AlertExpired", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "AlertExpired", c => c.Boolean(nullable: false));
        }
    }
}
