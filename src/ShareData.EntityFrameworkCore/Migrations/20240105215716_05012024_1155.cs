using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareData.Migrations
{
    public partial class _05012024_1155 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FormStages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "FormStages");
        }
    }
}
