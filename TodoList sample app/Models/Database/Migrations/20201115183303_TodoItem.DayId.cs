using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList_sample_app.Models.Database.Migrations
{
    public partial class TodoItemDayId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Days_DayId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Days_DayId",
                table: "Items",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Days_DayId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "DayId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Days_DayId",
                table: "Items",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
