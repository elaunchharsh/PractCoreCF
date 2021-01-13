using Microsoft.EntityFrameworkCore.Migrations;

namespace PractCoreCF.Migrations
{
    public partial class mig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserImages_UserId",
                table: "UserImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_UserMaster_UserId",
                table: "UserImages",
                column: "UserId",
                principalTable: "UserMaster",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_UserMaster_UserId",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_UserId",
                table: "UserImages");
        }
    }
}
