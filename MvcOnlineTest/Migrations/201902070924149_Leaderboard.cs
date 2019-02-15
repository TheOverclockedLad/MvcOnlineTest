namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Leaderboard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leaderboards",
                c => new
                    {
                        LeaderId = c.Int(nullable: false, identity: true),
                        LeaderName = c.String(),
                        LeaderScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LeaderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leaderboards");
        }
    }
}
