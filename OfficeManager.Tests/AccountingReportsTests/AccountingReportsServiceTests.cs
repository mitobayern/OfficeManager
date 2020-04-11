using Microsoft.EntityFrameworkCore;
using Moq;
using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Areas.Administration.ViewModels.Landlords;
using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Data;
using OfficeManager.Services;
using OfficeManager.ViewModels.AccountingReports;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.AccountingReportsTests
{
    public class AccountingReportsServiceTests
    {
        [Fact]
        public void TestIfAccountingReportIsGeneratedCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

            var accountingReportViewModel = accontingReportsService.GetAccountingReportViewModel("TenantName", "1 януари - 31 януари 2020 г.");

            accontingReportsService.GenerateAccountingReport(accountingReportViewModel);

            Assert.Equal(1, dbContext.AccountingReports.Count());
        }

        [Fact]
        public void TestIfGetAccountingReportViewModelReturnsCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

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
        public void TestIfGetAccountingReportbyIdIsRetrnedCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

            var accountingReportViewModel = accontingReportsService.GetAccountingReportViewModel("TenantName", "1 януари - 31 януари 2020 г.");

            accontingReportsService.GenerateAccountingReport(accountingReportViewModel);

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
        public void TestIfGetAllPeriodsAndTenantsAreRetrnedCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

            Assert.Single(accontingReportsService.GetAllTenantsSelectList().ToList());
            Assert.Single(accontingReportsService.GetAllPeriodsSelectList().ToList());
        }

        [Fact]
        public void TestIfGetTemperatureAndElectricityConsummationsAreRetrnedCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

            Assert.Equal(10M, accontingReportsService.GetTenantElectricityConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").DayTimeConsummation);
            Assert.Equal(5M, accontingReportsService.GetTenantElectricityConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").NightTimeConsummation);
            Assert.Equal(10M, accontingReportsService.GetTenantTemperatureConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").HeatingConsummation);
            Assert.Equal(5M, accontingReportsService.GetTenantTemperatureConsummationByPeriod("TenantName", "1 януари - 31 януари 2020 г.").CoolingConsummation);
        }

        [Fact]
        public void TestIfAmountForElectricityIsRetrnedCorrectrly()
        {
            using var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions());
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IAccontingReportsService accontingReportsService =
                new AccountingReportsService(dbContext, tenantsService, landlordsService, pricesInformationService);

            SeedData(dbContext);

            var electricityConsummation = new TenantElectricityConsummationViewModel
            {
                DayTimeConsummation = 10M,
                NightTimeConsummation = 5M,            
            };

            Assert.Equal(165, accontingReportsService.AmountForElectricity(electricityConsummation));
        }

        private static void SeedData(ApplicationDbContext dbContext)
        {
            ITenantsService tenantsService = new TenantsService(dbContext);
            ILandlordsService landlordsService = new LandlordsService(dbContext);
            IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);
            ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);
            IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);
            IMeasurementsService measurementsService = new MeasurementsService(dbContext, electricityMetersService, temperatureMetersService, tenantsService);
            IOfficesService officesService = new OfficesService(dbContext, tenantsService, electricityMetersService, temperatureMetersService);

            CreateLandlord(landlordsService);
            CreateTenant(tenantsService);
            CreateOffice(officesService);
            officesService.AddOfficesToTenantAsync(1, new List<string> { "TestOfficeName" });
            CreateTemperatureMeter(temperatureMetersService);
            CreateElectricityMeter(electricityMetersService);
            officesService.AddElectricityMeterToOfficeAsync(1, "ElectricityMeterName");
            officesService.AddTemperatureMetersToOfficeAsync(1, new List<string> { "TemperatureMeterName" });
            CreateInitialElectricityMeasuremets(measurementsService);
            CreateElectricityMeasuremets(measurementsService);
            CreateInitialTemperatureMeasurements(measurementsService);
            CreateTemperatureMeasuremets(measurementsService);
            CreatePricelist(pricesInformationService);
        }

        private static void CreateLandlord(ILandlordsService landlordsService)
        {
            landlordsService.CreateLandlordAsync(new CreateLandlordViewModel
            {
                LandlordName = "LandlordName",
                LandlordOwner = "LandlordOwner",
                Bulstat = "BG123456789",
                Address = "LandlordAddress",
                Email = "Landlord@email.com",
                Phone = "0888888888"
            });
        }

        private static void CreateTenant(ITenantsService tenantsService)
        {
            tenantsService.CreateTenantAsync(new CreateTenantViewModel
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

        private static void CreateOffice(IOfficesService officesService)
        {
            officesService.CreateOfficeAsync(new CreateOfficeViewModel
            {
                Name = "TestOfficeName",
                Area = 100M,
                RentPerSqMeter = 7.2M
            });
        }

        private static void CreateTemperatureMeter(ITemperatureMetersService temperatureMetersService)
        {
            temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
            {
                Name = "TemperatureMeterName"
            });
        }

        private static void CreateElectricityMeter(IElectricityMetersService electricityMetersService)
        {
            electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
            {
                Name = "ElectricityMeterName",
                PowerSupply = 10M,
            });
        }

        private static void CreateInitialElectricityMeasuremets(IMeasurementsService measurementsService)
        {
            measurementsService.CreateInitialElectricityMeasurementAsync(new DateTime(2020, 1, 1), "ElectricityMeterName", 0M, 0M);
        }

        private static void CreateElectricityMeasuremets(IMeasurementsService measurementsService)
        {
            measurementsService.CreateElectricityMeasurementAsync(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "ElectricityMeterName", 10M, 5M);
        }

        private static void CreateInitialTemperatureMeasurements(IMeasurementsService measurementsService)
        {
            measurementsService.CreateInitialTemperatureMeasurementAsync(new DateTime(2020, 1, 1), "TemperatureMeterName", 0M, 0M);
        }

        private static void CreateTemperatureMeasuremets(IMeasurementsService measurementsService)
        {
            measurementsService.CreateTemperatureMeasurementAsync(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "TemperatureMeterName", 10M, 5M);
        }

        private static void CreatePricelist(IPricesInformationService pricesInformationService)
        {
            pricesInformationService.CreatePricelistAsync(new CreatePricesInputViewModel
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
