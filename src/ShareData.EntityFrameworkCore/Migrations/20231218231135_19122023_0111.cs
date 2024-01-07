using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareData.Migrations
{
    public partial class _19122023_0111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkFlows",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkFlows");
        }
    }
}
