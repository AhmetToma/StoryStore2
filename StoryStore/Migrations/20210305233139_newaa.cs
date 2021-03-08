using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryStore.Migrations
{
    public partial class newaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgeRangeId",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stories_AgeRangeId",
                table: "Stories",
                column: "AgeRangeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AgeRanges_AgeRangeId",
                table: "Stories",
                column: "AgeRangeId",
                principalTable: "AgeRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AgeRanges_AgeRangeId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Stories_AgeRangeId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "AgeRangeId",
                table: "Stories");
        }
    }
}
