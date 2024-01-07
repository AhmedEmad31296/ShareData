using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareData.Migrations
{
    public partial class _26122023_2300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkFlowStageStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelStatus = table.Column<int>(type: "int", nullable: false),
                    WorkFlowStageId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WorkFlowStageStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowStageStatus_WorkFlowStages_ParentId",
                        column: x => x.ParentId,
                        principalTable: "WorkFlowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_WorkFlowStageStatus_WorkFlowStages_WorkFlowStageId",
                        column: x => x.WorkFlowStageId,
                        principalTable: "WorkFlowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStageStatus_ParentId",
                table: "WorkFlowStageStatus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStageStatus_WorkFlowStageId",
                table: "WorkFlowStageStatus",
                column: "WorkFlowStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkFlowStageStatus");
        }
    }
}
