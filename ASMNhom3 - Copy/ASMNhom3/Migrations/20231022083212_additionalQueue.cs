using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMNhom3.Migrations
{
    public partial class additionalQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "QueueCheckOuts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "QueueCheckOuts");
        }
    }
}
