using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackToAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Attempts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Attempts");
        }
    }
}
