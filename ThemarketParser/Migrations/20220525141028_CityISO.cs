using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThemarketParser.Migrations
{
    public partial class CityISO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "iso",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iso",
                table: "Cities");
        }
    }
}
