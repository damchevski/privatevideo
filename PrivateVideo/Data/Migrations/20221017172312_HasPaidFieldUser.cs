using Microsoft.EntityFrameworkCore.Migrations;

namespace PrivateVideo.Data.Migrations
{
    public partial class HasPaidFieldUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPaid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPaid",
                table: "AspNetUsers");
        }
    }
}
