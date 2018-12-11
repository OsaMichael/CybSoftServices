namespace CybSoftServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Servers", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Servers", "ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Servers", "ModifiedDate", c => c.DateTime());
            AlterColumn("dbo.Servers", "CreatedDate", c => c.DateTime());
        }
    }
}
