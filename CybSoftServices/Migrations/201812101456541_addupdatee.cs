namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addupdatee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "Services", c => c.String());
            AlterColumn("dbo.Services", "ExpiringDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "ExpiringDate", c => c.String(nullable: false));
            AlterColumn("dbo.Services", "Services", c => c.String(nullable: false));
        }
    }
}
