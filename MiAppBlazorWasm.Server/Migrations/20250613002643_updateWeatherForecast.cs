using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiAppBlazorWasm.Server.Migrations
{
    /// <inheritdoc />
    public partial class updateWeatherForecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "WeatherForecasts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "WeatherForecasts");
        }
    }
}
