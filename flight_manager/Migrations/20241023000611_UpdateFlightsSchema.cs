using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flight_manager.Migrations
{
    public partial class UpdateFlightsSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity_Buissness",
                table: "Flights",
                newName: "Capacity_Business");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity_Business",
                table: "Flights",
                newName: "Capacity_Buissness");
        }
    }
}
