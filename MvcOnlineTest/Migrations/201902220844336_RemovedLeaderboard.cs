namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedLeaderboard : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Leaderboards");
        }
        
        public override void Down()
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
    }
}
