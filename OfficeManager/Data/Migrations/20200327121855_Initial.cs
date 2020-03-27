using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeManager.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landlords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: false),
                    CompanyOwner = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Bulstat = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricesInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    HeatingPerKWh = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CoolingPerKWh = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ElectricityPerKWh = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Excise = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AccessToDistributionGrid = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NetworkTaxesAndUtilities = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: false),
                    CompanyOwner = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Bulstat = table.Column<string>(nullable: false),
                    StartOfContract = table.Column<DateTime>(nullable: false),
                    EndOfContract = table.Column<DateTime>(nullable: true),
                    hasContract = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountingReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    LandlordId = table.Column<int>(nullable: false),
                    PricesInformationId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HeatingConsummation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CoolingConsummation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DayTimeElectricityConsummation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NightTimeElectricityConsummation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountForHeating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountForCooling = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountForElectricity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountForCleaning = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Period = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingReports_Landlords_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountingReports_PricesInformation_PricesInformationId",
                        column: x => x.PricesInformationId,
                        principalTable: "PricesInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountingReports_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    LandlordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Tenants_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Landlords_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentPerSqMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isAvailable = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDescriptions_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityMeters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    PowerSupply = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfficeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityMeters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityMeters_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureMeters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    OfficeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureMeters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemperatureMeters_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElectricityMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayTimeMeasurement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NightTimeMeasurement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StartOfPeriod = table.Column<DateTime>(nullable: false),
                    EndOfPeriod = table.Column<DateTime>(nullable: false),
                    Period = table.Column<string>(nullable: false),
                    ElectricityMeterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricityMeasurements_ElectricityMeters_ElectricityMeterId",
                        column: x => x.ElectricityMeterId,
                        principalTable: "ElectricityMeters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeatingMeasurement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CoolingMeasurement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartOfPeriod = table.Column<DateTime>(nullable: false),
                    EndOfPeriod = table.Column<DateTime>(nullable: false),
                    Period = table.Column<string>(nullable: false),
                    TemperatureMeterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemperatureMeasurements_TemperatureMeters_TemperatureMeterId",
                        column: x => x.TemperatureMeterId,
                        principalTable: "TemperatureMeters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingReports_LandlordId",
                table: "AccountingReports",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingReports_PricesInformationId",
                table: "AccountingReports",
                column: "PricesInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingReports_TenantId",
                table: "AccountingReports",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityMeasurements_ElectricityMeterId",
                table: "ElectricityMeasurements",
                column: "ElectricityMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityMeters_OfficeId",
                table: "ElectricityMeters",
                column: "OfficeId",
                unique: true,
                filter: "[OfficeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDescriptions_InvoiceId",
                table: "InvoiceDescriptions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LandlordId",
                table: "Invoices",
                column: "LandlordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_TenantId",
                table: "Offices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureMeasurements_TemperatureMeterId",
                table: "TemperatureMeasurements",
                column: "TemperatureMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureMeters_OfficeId",
                table: "TemperatureMeters",
                column: "OfficeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingReports");

            migrationBuilder.DropTable(
                name: "ElectricityMeasurements");

            migrationBuilder.DropTable(
                name: "InvoiceDescriptions");

            migrationBuilder.DropTable(
                name: "TemperatureMeasurements");

            migrationBuilder.DropTable(
                name: "PricesInformation");

            migrationBuilder.DropTable(
                name: "ElectricityMeters");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "TemperatureMeters");

            migrationBuilder.DropTable(
                name: "Landlords");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
