using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareData.Migrations
{
    public partial class _16012024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkFlowStageUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkFlowStageId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowStageUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowStageUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkFlowStageUsers_WorkFlowStages_WorkFlowStageId",
                        column: x => x.WorkFlowStageId,
                        principalTable: "WorkFlowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStageUsers_UserId",
                table: "WorkFlowStageUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStageUsers_WorkFlowStageId",
                table: "WorkFlowStageUsers",
                column: "WorkFlowStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkFlowStageUsers");
        }
    }
}
