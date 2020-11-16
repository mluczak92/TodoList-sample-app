using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList_sample_app.Models.Database.Migrations
{
    public partial class TodoItemReminderTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderTime",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ReminderTime",
                table: "Items",
                column: "ReminderTime")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_ReminderTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Items");
        }
    }
}
