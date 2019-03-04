namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTestAddTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "time", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "time");
        }
    }
}
