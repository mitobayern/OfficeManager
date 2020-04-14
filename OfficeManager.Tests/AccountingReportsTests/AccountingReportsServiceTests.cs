namespace OfficeManager.Tests.AccountingReportsTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.Measurements;
    using Xunit;

    public class AccountingReportsServiceTests
    {
        [Fact]
        public async Task TestIfAccountingReportIsGeneratedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            var accountingReportViewModel = accontingReportsService.GetAccountingReportViewModel("TenantName", "1 януари - 31 януари 2020 г.");

            await accontingReportsService.GenerateAccountingReportAsync(accountingReportViewModel);

            Assert.Equal(1, dbContext.AccountingReports.Count());
        }

        [Fact]
        public async Task TestIfGetAccountingReportViewModelReturnsCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            var accountingReportViewModel = accontingReportsService.GetAccountingReportViewModel("TenantName", "1 януари - 31 януари 2020 г.");

            Assert.Equal("1 януари - 31 януари 2020 г.", accountingReportViewModel.Period);
            Assert.Equal("TenantName", accountingReportViewModel.Tenant.CompanyName);
            Assert.Equal("LandlordOwner", accountingReportViewModel.Landlord.LandlordOwner);
            Assert.Equal(165M, accountingReportViewModel.AmountForElectricity);
            Assert.Equal(100M, accountingReportViewModel.AmountForHeating);
            Assert.Equal(50M, accountingReportViewModel.AmountForCooling);
            Assert.Equal(315M * 1.2M, accountingReportViewModel.TotalAmount);
        }

        [Fact]
        public async Task TestIfGetAccountingReportbyIdIsRetrnedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            var accountingReportViewModel = accontingReportsService.GetAccountingReportViewModel("TenantName", "1 януари - 31 януари 2020 г.");

            await accontingReportsService.GenerateAccountingReportAsync(accountingReportViewModel);

            var result = accontingReportsService.GetAccountingReportById(1);

            Assert.Single(accontingReportsService.GetAllAccountingReports().ToList());
            Assert.Equal("1 януари - 31 януари 2020 г.", result.Period);
            Assert.Equal("TenantName", result.Tenant.CompanyName);
            Assert.Equal("офис TestOfficeName", result.Tenant.Offices);
            Assert.Equal("LandlordOwner", result.Landlord.LandlordOwner);
            Assert.Equal(165M, result.AmountForElectricity);
            Assert.Equal(315M * 1.2M, result.TotalAmount);
        }

        [Fact]
        public async Task TestIfGetAllPeriodsAndTenantsAreRetrnedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            Assert.Single(accontingReportsService.GetAllTenantsSelectList().ToList());
            Assert.Single(accontingReportsService.GetAllPeriodsSelectList().ToList());
        }

        [Fact]
        public async Task TestIfGetTemperatureAndElectricityConsummationsAreRetrnedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            Assert.Equal(10M, accontingReportsService.GetTenantElectricityConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").DayTimeConsummation);
            Assert.Equal(5M, accontingReportsService.GetTenantElectricityConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").NightTimeConsummation);
            Assert.Equal(10M, accontingReportsService.GetTenantTemperatureConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").HeatingConsummation);
            Assert.Equal(5M, accontingReportsService.GetTenantTemperatureConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").CoolingConsummation);
        }

        [Fact]
        public async Task TestIfAmountForElectricityIsRetrnedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccountingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            await SeedDataAsync(dbContext);

            var electricityConsummation = new TenantElectricityConsummationViewModel
            {
                DayTimeConsummation = 10M,
                NightTimeConsummation = 5M,
            };

            Assert.Equal(165, accontingReportsService.AmountForElectricity(electricityConsummation));
        }

        private static async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);
            ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IMeasurementsService measurementsService = new MeasurementsService(dbContext, electricityMetersService, temperatureMetersService);
            IOfficesService officesService = new OfficesService(dbContext, tenantsService, electricityMetersService, temperatureMetersService);

            await CreateLandlordAsync(landlordsService);
            await CreateTenantAsync(tenantsService);
            await CreateOfficeAsync(officesService);
            await officesService.AddOfficesToTenantAsync(1, new List<string> { "TestOfficeName" });
            await CreateTemperatureMeterAsync(temperatureMetersService);
            await CreateElectricityMeterAsync(electricityMetersService);
            await officesService.AddElectricityMeterToOfficeAsync(1, "ElectricityMeterName");
            await officesService.AddTemperatureMetersToOfficeAsync(1, new List<string> { "TemperatureMeterName" });
            await CreateInitialElectricityMeasuremetsAsync(measurementsService);
            await CreateElectricityMeasuremetsAsync(measurementsService);
            await CreateInitialTemperatureMeasurementsAsync(measurementsService);
            await CreateTemperatureMeasuremetsAsync(measurementsService);
            await CreatePricelistAsync(pricesInformationService);
        }

        private static async Task CreateLandlordAsync(ILandlordsService landlordsService)
        {
            await landlordsService.CreateLandlordAsync(new CreateLandlordViewModel
            {
                LandlordName = "LandlordName",
                LandlordOwner = "LandlordOwner",
                Bulstat = "BG123456789",
                Address = "LandlordAddress",
                Email = "Landlord@email.com",
                Phone = "0888888888",
            });
        }

        private static async Task CreateTenantAsync(ITenantsService tenantsService)
        {
            await tenantsService.CreateTenantAsync(new CreateTenantViewModel
            {
                CompanyName = "TenantName",
                CompanyOwner = "TenantOwner",
                Bulstat = "123456789",
                Address = "TenantAddress",
                Email = "tenant@email.com",
                Phone = "0999999999",
                StartOfContract = DateTime.UtcNow,
            });
        }

        private static async Task CreateOfficeAsync(IOfficesService officesService)
        {
            await officesService.CreateOfficeAsync("TestOfficeName", 100M, 7.2M);
        }

        private static async Task CreateTemperatureMeterAsync(ITemperatureMetersService temperatureMetersService)
        {
            await temperatureMetersService.CreateTemperatureMeterAsync("TemperatureMeterName");
        }

        private static async Task CreateElectricityMeterAsync(IElectricityMetersService electricityMetersService)
        {
            await electricityMetersService.CreateElectricityMeterAsync("ElectricityMeterName", 10M);
        }

        private static async Task CreateInitialElectricityMeasuremetsAsync(IMeasurementsService measurementsService)
        {
            await measurementsService.CreateInitialElectricityMeasurementAsync(new DateTime(2020, 1, 1), "ElectricityMeterName", 0M, 0M);
        }

        private static async Task CreateElectricityMeasuremetsAsync(IMeasurementsService measurementsService)
        {
            await measurementsService.CreateElectricityMeasurementAsync(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "ElectricityMeterName", 10M, 5M);
        }

        private static async Task CreateInitialTemperatureMeasurementsAsync(IMeasurementsService measurementsService)
        {
            await measurementsService.CreateInitialTemperatureMeasurementAsync(new DateTime(2020, 1, 1), "TemperatureMeterName", 0M, 0M);
        }

        private static async Task CreateTemperatureMeasuremetsAsync(IMeasurementsService measurementsService)
        {
            await measurementsService.CreateTemperatureMeasurementAsync(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "TemperatureMeterName", 10M, 5M);
        }

        private static async Task CreatePricelistAsync(IPricesInformationService pricesInformationService)
        {
            await pricesInformationService.CreatePricelistAsync(new CreatePricesInputViewModel
            {
                ElectricityPerKWh = 10M,
                HeatingPerKWh = 10M,
                CoolingPerKWh = 10M,
                AccessToDistributionGrid = 0.5M,
                NetworkTaxesAndUtilities = 0.3M,
                Excise = 0.2M,
            });
        }

        private DbContextOptions<ApplicationDbContext> GetInMemoryDadabaseOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return options;
        }
    }
}
