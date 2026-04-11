using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChildBinCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Parents");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Children");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Parents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
