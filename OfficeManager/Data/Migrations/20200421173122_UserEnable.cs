using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeManager.Data.Migrations
{
    public partial class UserEnable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hasContract",
                table: "Tenants",
                newName: "HasContract");

            migrationBuilder.RenameColumn(
                name: "isAvailable",
                table: "Offices",
                newName: "IsAvailable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasContract",
                table: "Tenants",
                newName: "hasContract");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Offices",
                newName: "isAvailable");
        }
    }
}
