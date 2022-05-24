using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThemarketParser.Migrations
{
    public partial class ItemIsModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isModified",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isModified",
                table: "Items");
        }
    }
}
