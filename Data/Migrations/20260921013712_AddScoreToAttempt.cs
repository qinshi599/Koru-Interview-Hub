using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreToAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Attempts",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Attempts");
        }
    }
}
