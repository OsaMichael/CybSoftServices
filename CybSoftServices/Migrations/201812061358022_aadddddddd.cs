namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aadddddddd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "AlertExpired", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "AlertExpired", c => c.Boolean(nullable: false));
        }
    }
}
