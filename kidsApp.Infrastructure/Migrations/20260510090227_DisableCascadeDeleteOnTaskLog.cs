using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kidsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisableCascadeDeleteOnTaskLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Tasks_TaskId",
                table: "TaskLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Tasks_TaskId",
                table: "TaskLogs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Tasks_TaskId",
                table: "TaskLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Tasks_TaskId",
                table: "TaskLogs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
