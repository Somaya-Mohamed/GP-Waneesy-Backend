using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsRewardedToTaskAndStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsRewarded",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PointsRewarded",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsRewarded",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PointsRewarded",
                table: "Stories");
        }
    }
}
