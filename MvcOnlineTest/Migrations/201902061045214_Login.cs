namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Login : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        username = c.String(nullable: false, maxLength: 128),
                        password = c.String(),
                        roles = c.String(),
                    })
                .PrimaryKey(t => t.username);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logins");
        }
    }
}
