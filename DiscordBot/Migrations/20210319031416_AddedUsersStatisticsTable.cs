using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class AddedUsersStatisticsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersStatistics",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    MessagesSent = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersStatistics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersStatistics_GuildId_Username",
                table: "UsersStatistics",
                columns: new[] { "GuildId", "Username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersStatistics_Id",
                table: "UsersStatistics",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersStatistics");
        }
    }
}
