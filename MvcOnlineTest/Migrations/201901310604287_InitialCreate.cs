namespace MvcOnlineTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        TestId = c.Int(),
                        Que = c.String(nullable: false),
                        OptionA = c.String(),
                        OptionB = c.String(),
                        OptionC = c.String(),
                        OptionD = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Tests", t => t.TestId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(),
                        TestName = c.String(nullable: false, maxLength: 30),
                        MarksPerQue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 25),
                        FirstName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.StudentTests",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.TestId })
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.TestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTests", "TestId", "dbo.Tests");
            DropForeignKey("dbo.StudentTests", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Tests", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Questions", "TestId", "dbo.Tests");
            DropIndex("dbo.StudentTests", new[] { "TestId" });
            DropIndex("dbo.StudentTests", new[] { "StudentId" });
            DropIndex("dbo.Tests", new[] { "StudentId" });
            DropIndex("dbo.Questions", new[] { "TestId" });
            DropTable("dbo.StudentTests");
            DropTable("dbo.Students");
            DropTable("dbo.Tests");
            DropTable("dbo.Questions");
        }
    }
}
