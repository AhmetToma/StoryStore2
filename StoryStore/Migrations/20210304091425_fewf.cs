using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryStore.Migrations
{
    public partial class fewf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AgeRenges_AgeRangeId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgeRenges",
                table: "AgeRenges");

            migrationBuilder.RenameTable(
                name: "AgeRenges",
                newName: "AgeRanges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgeRanges",
                table: "AgeRanges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AgeRanges_AgeRangeId",
                table: "AspNetUsers",
                column: "AgeRangeId",
                principalTable: "AgeRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AgeRanges_AgeRangeId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgeRanges",
                table: "AgeRanges");

            migrationBuilder.RenameTable(
                name: "AgeRanges",
                newName: "AgeRenges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgeRenges",
                table: "AgeRenges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AgeRenges_AgeRangeId",
                table: "AspNetUsers",
                column: "AgeRangeId",
                principalTable: "AgeRenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
