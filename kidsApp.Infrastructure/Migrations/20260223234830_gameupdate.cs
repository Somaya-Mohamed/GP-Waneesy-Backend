using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class gameupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
