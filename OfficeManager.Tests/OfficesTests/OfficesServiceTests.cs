namespace OfficeManager.Tests.OfficesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.Services;
    using Xunit;

    public class OfficesServiceTests
    {
        private readonly string officeName;
        private readonly decimal officeArea;
        private readonly decimal officeRent;
        private CreateTenantViewModel inputTenant;
        private Mock<ITenantsService> tenantsService;
        private Mock<IElectricityMetersService> electricityMetersService;
        private Mock<ITemperatureMetersService> temperatureMetersService;

        public OfficesServiceTests()
        {
            this.officeName = "TestOfficeName";
            this.officeArea = 50M;
            this.officeRent = 7.2M;
            this.inputTenant = new CreateTenantViewModel
            {
                CompanyName = "TestCompanyName",
                CompanyOwner = "TestCompanyOwner",
                Bulstat = "123456789",
                Address = "TestAddress",
                Email = "test@email.com",
                Phone = "0888888888",
                StartOfContract = DateTime.UtcNow,
            };
            this.tenantsService = new Mock<ITenantsService>();
            this.electricityMetersService = new Mock<IElectricityMetersService>();
            this.temperatureMetersService = new Mock<ITemperatureMetersService>();
        }

        [Fact]
        public async Task TestIfOfficeIsCreatedCorrectlyAsync()
        {
            int actualOfficesCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                await officesService.CreateOfficeAsync("FirstTestOfficeName", 50M, 7.2M);

                for (int i = 0; i < 3; i++)
                {
                    await officesService.CreateOfficeAsync("SecondTestOfficeName", 50M, 7.2M);
                }

                actualOfficesCount = dbContext.Offices.Count();
            }

            Assert.Equal(2, actualOfficesCount);
        }

        [Fact]
        public async Task TestIfOfficeIsUpdatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IOfficesService officesService = new OfficesService(
                dbContext,
                this.tenantsService.Object,
                this.electricityMetersService.Object,
                this.temperatureMetersService.Object);

            await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);
            await officesService.UpdateOfficeAsync(1, "UpdatedOffice", 20M, 10M);

            Assert.Equal("UpdatedOffice", officesService.GetOfficeById(1).Name);
            Assert.Equal(20M, officesService.GetOfficeByName("UpdatedOffice").Area);
            Assert.Equal(10M, officesService.GetOfficeByName("UpdatedOffice").RentPerSqMeter);
        }

        [Fact]
        public async Task TestIfOfficeIsReturnedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IOfficesService officesService = new OfficesService(
                dbContext,
                this.tenantsService.Object,
                this.electricityMetersService.Object,
                this.temperatureMetersService.Object);

            await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);

            Assert.Equal("No electricity meter available", officesService.EditOffice(1).ElectricityMeter);
            Assert.Equal(50M, officesService.EditOffice(1).Area);
            Assert.Equal(7.2M, officesService.EditOffice(1).RentPerSqMeter);
            Assert.Equal(this.officeName, officesService.EditOffice(1).Name);
            Assert.Equal(1, officesService.EditOffice(1).Id);
        }

        [Fact]
        public async Task TestIfGetOfficeByIdReturnsCorrectrlyAsync()
        {
            string officeName;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);
                officeName = officesService.GetOfficeById(1).Name;
            }

            Assert.Equal("TestOfficeName", officeName);
        }

        [Fact]
        public async Task TestIfGetOfficeByNameReturnsCorrectrlyAsync()
        {
            string officeName;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);

                officeName = officesService.GetOfficeByName("TestOfficeName").Name;
            }

            Assert.Equal("TestOfficeName", officeName);
        }

        [Fact]
        public async Task TestIfOfficesAreAddedAndRemovedToTenantCorectlyAsync()
        {
            int tenantOfficesCount;
            List<string> officesToAdd = new List<string> { "1", "3", "5" };
            List<string> officesToRemove = new List<string> { "3" };

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenants = new TenantsService(dbContext);

                IOfficesService officesService = new OfficesService(
                    dbContext,
                    tenants,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                await AddOfficesToTenantAsync(this.inputTenant, tenants, officesService);
                await officesService.AddOfficesToTenantAsync(1, officesToAdd);
                await officesService.RemoveOfficesFromTenantAsync(1, officesToRemove);
                tenantOfficesCount = dbContext.Tenants.FirstOrDefault(x => x.CompanyName == "TestCompanyName").Offices.Count();
            }

            Assert.Equal(2, tenantOfficesCount);
        }

        [Fact]
        public async Task TestIfAvailableOfficesAreReturnedCorectlyAsync()
        {
            int availableOfficesCount;
            List<string> offices = new List<string> { "1", "3" };
            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenants = new TenantsService(dbContext);

                IOfficesService officesService = new OfficesService(
                    dbContext,
                    tenants,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                await AddOfficesToTenantAsync(this.inputTenant, tenants, officesService);

                await officesService.AddOfficesToTenantAsync(1, offices);
                var officesCount = dbContext.Offices.Count();
                availableOfficesCount = officesService.GetAllAvailableOffices().Count();
            }

            Assert.Equal(3, availableOfficesCount);
        }

        [Fact]
        public async Task TestIfAllOfficesAreReturnedCorrectrlyAsync()
        {
            string names = string.Empty;
            List<OfficeOutputViewModel> allOffices = new List<OfficeOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                for (int i = 1; i <= 3; i++)
                {
                    await officesService.CreateOfficeAsync(i.ToString(), this.officeArea, this.officeRent);
                }

                allOffices = officesService.GetAllOffices().ToList();
            }

            foreach (var office in allOffices)
            {
                names += office.Name;
            }

            Assert.Equal(3, allOffices.Count);
            Assert.Equal("123", names);
        }

        [Fact]
        public async Task TestIfElectricityMeterIsAddedAndRemovedToOfficeCorrectlyAsync()
        {
            string actualElectricityMetersName;

            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);

            IOfficesService officesService = new OfficesService(
                dbContext,
                this.tenantsService.Object,
                electricityMeters,
                this.temperatureMetersService.Object);

            await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);
            await electricityMeters.CreateElectricityMeterAsync("TestElectricityMeter", 5M);
            await officesService.AddElectricityMeterToOfficeAsync(1, "TestElectricityMeter");
            actualElectricityMetersName = officesService.GetOfficeByName("TestOfficeName").ElectricityMeter.Name;
            await officesService.RemoveElectricityMeterFromOfficeAsync(1);

            Assert.Throws<NullReferenceException>(() => officesService.GetOfficeByName("TestOfficeName").ElectricityMeter.Name);
        }

        [Fact]
        public async Task TestIfTemperatureMetersAreAddedAndRemovedToOfficeCorrectlyAsync()
        {
            int temperatureMetersCount;
            List<string> temperatureMetersToAdd = new List<string> { "1", "3", "5" };
            List<string> temperatureMetersToRemove = new List<string> { "3" };
            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);

                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    temperatureMeters);

                await officesService.CreateOfficeAsync(this.officeName, this.officeArea, this.officeRent);

                for (int i = 1; i <= 5; i++)
                {
                    await temperatureMeters.CreateTemperatureMeterAsync(i.ToString());
                }

                var office = officesService.GetOfficeById(1);
                var count = dbContext.TemperatureMeters.Count();
                await officesService.AddTemperatureMetersToOfficeAsync(1, temperatureMetersToAdd);
                await officesService.RemoveTemperatureMetersFromOfficeAsync(1, temperatureMetersToRemove);
                temperatureMetersCount = officesService.GetOfficeTemperatureMeters(1).ToList().Count();
                Assert.Equal(2, officesService.EditOffice(1).TemperatureMeters.Count());

            }

            Assert.Equal(2, temperatureMetersCount);
        }

        [Fact]
        public async Task TestIfOfficeIsDeletedCorrectlyAsync()
        {
            int actualOfficesCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService = new OfficesService(
                    dbContext,
                    this.tenantsService.Object,
                    this.electricityMetersService.Object,
                    this.temperatureMetersService.Object);

                for (int i = 1; i <= 3; i++)
                {
                    await officesService.CreateOfficeAsync("OfficeName" + i.ToString(), 50M, 7.2M);
                }

                await officesService.DeleteOfficeAsync(2);

                actualOfficesCount = dbContext.Offices.Count();
            }

            Assert.Equal(2, actualOfficesCount);
        }

        private static async Task AddOfficesToTenantAsync(CreateTenantViewModel inputTenant, ITenantsService tenants, IOfficesService officesService)
        {
            await tenants.CreateTenantAsync(inputTenant);

            for (int i = 1; i <= 5; i++)
            {
                await officesService.CreateOfficeAsync(i.ToString(), 50M, 7.2M);
            }
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
