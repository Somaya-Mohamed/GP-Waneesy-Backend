using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsRewardedToVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsRewarded",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsRewarded",
                table: "Videos");
        }
    }
}
