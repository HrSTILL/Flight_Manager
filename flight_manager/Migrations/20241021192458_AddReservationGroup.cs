using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flight_manager.Migrations
{
    public partial class AddReservationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Flight_Number_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Location_From = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Location_To = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Date_Hour_Takeoff = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date_Hour_Landing = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Plane_Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Plane_Number = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Pilot_Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Capacity_Normal = table.Column<int>(type: "INTEGER", nullable: false),
                    Capacity_Buissness = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Flight_Number_id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Reservation_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LeaderEmail = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    First_Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Middle_Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Last_Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EGN = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Phone_Number = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Nationality = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Flight_Number_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Ticket_Type = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Reservation_Status = table.Column<string>(type: "TEXT", nullable: false),
                    Reservation_Group = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Reservation_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Reservations");

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
    }
}
