namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTestQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestQuestions",
                c => new
                    {
                        TestId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TestId, t.QuestionId })
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestQuestions", "TestId", "dbo.Tests");
            DropForeignKey("dbo.TestQuestions", "QuestionId", "dbo.Questions");
            DropIndex("dbo.TestQuestions", new[] { "QuestionId" });
            DropIndex("dbo.TestQuestions", new[] { "TestId" });
            DropTable("dbo.TestQuestions");
        }
    }
}
