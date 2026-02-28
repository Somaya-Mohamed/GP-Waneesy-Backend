using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adddiffiultytotasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Tasks");
        }
    }
}
