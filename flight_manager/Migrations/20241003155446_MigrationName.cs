using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flight_manager.Migrations
{
    public partial class MigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginTokens",
                table: "LoginTokens");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LoginTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginTokens",
                table: "LoginTokens",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginTokens",
                table: "LoginTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LoginTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginTokens",
                table: "LoginTokens",
                column: "Token");
        }
    }
}
