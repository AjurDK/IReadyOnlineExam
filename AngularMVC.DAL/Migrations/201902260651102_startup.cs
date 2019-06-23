namespace AngularMVC.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class startup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                {
                    AnswerID = c.Int(nullable: false, identity: true),
                    AnswerText = c.String(),
                    QuestionID = c.Int(),
                    IsAnwer = c.Boolean(),
                })
                .PrimaryKey(t => t.AnswerID)
                .ForeignKey("dbo.Questions", t => t.QuestionID)
                .Index(t => t.QuestionID);

            CreateTable(
                "dbo.Questions",
                c => new
                {
                    QuestionID = c.Int(nullable: false, identity: true),
                    QuestionText = c.String(),
                    QuizID = c.Int(),
                    Delay = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Quizs", t => t.QuizID)
                .Index(t => t.QuizID);

            CreateTable(
                "dbo.Quizs",
                c => new
                {
                    QuizID = c.Int(nullable: false, identity: true),
                    QuizName = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.QuizID);

            CreateTable(
                "dbo.QuizLogs",
                c => new
                {
                    ID = c.Long(nullable: false, identity: true),
                    UserID = c.Guid(nullable: false),
                    QuestionID = c.Int(nullable: false),
                    AnswerID = c.Int(nullable: false),
                    PostTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Answers", t => t.AnswerID)
                .ForeignKey("dbo.Questions", t => t.QuestionID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.QuestionID)
                .Index(t => t.AnswerID);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserID = c.Guid(nullable: false),
                    Username = c.String(),
                    Password = c.String(),
                })
                .PrimaryKey(t => t.UserID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.QuizLogs", "UserID", "dbo.Users");
            DropForeignKey("dbo.QuizLogs", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.QuizLogs", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.Questions", "QuizID", "dbo.Quizs");
            DropForeignKey("dbo.Answers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizLogs", new[] { "AnswerID" });
            DropIndex("dbo.QuizLogs", new[] { "QuestionID" });
            DropIndex("dbo.QuizLogs", new[] { "UserID" });
            DropIndex("dbo.Questions", new[] { "QuizID" });
            DropIndex("dbo.Answers", new[] { "QuestionID" });
            DropTable("dbo.Users");
            DropTable("dbo.QuizLogs");
            DropTable("dbo.Quizs");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
