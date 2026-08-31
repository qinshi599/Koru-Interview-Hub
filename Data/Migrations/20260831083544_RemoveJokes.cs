using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJokes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Joke");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Joke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JokeAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    JokeQuestion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Joke", x => x.Id);
                });
        }
    }
}
