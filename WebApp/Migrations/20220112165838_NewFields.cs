using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PollQuestions",
                table: "AnsweredQuizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizQuestionsCorrect",
                table: "AnsweredQuizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizQuestionsTotal",
                table: "AnsweredQuizzes",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PollQuestions",
                table: "AnsweredQuizzes");

            migrationBuilder.DropColumn(
                name: "QuizQuestionsCorrect",
                table: "AnsweredQuizzes");

            migrationBuilder.DropColumn(
                name: "QuizQuestionsTotal",
                table: "AnsweredQuizzes");
        }
    }
}
