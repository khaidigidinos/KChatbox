using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRApi.Migrations.UserDatabase
{
    public partial class MigrationVer4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseTokens",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseTokens",
                table: "Users");
        }
    }
}
