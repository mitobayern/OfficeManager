namespace OfficeManager.Tests.ElectricityMetersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using Xunit;

    public class ElectricityMetersServiceTests
    {
        private readonly string name;
        private readonly decimal powerSupply;

        public ElectricityMetersServiceTests()
        {
            this.name = "Test";
            this.powerSupply = 10M;
        }

        [Fact]
        public async Task TestIfElectricityMeterIsCreatedCorrectlyAsync()
        {
            int actualElectricityMetersCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                await electricityMetersService.CreateElectricityMeterAsync("TestName1", this.powerSupply);

                for (int i = 0; i < 3; i++)
                {
                    await electricityMetersService.CreateElectricityMeterAsync("TestName2", this.powerSupply);
                }

                actualElectricityMetersCount = dbContext.ElectricityMeters.Count();
            }

            Assert.Equal(2, actualElectricityMetersCount);
        }

        [Fact]
        public async Task TestIfAllElectricityMetersAreReturnedCorrectrlyAsync()
        {
            string names = string.Empty;
            List<ElectricityMeterOutputViewModel> electricityMeters = new List<ElectricityMeterOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await electricityMetersService.CreateElectricityMeterAsync(i.ToString(), this.powerSupply);
                }

                electricityMeters = electricityMetersService.GetAllElectricityMeters().ToList();
            }

            foreach (var electricityMeter in electricityMeters)
            {
                names += electricityMeter.Name;
            }

            Assert.Equal(3, electricityMeters.Count);
            Assert.Equal("123", names);
        }

        [Fact]
        public async Task TestIfGetElectricityMeterByIdWorksCorrectlyAsync()
        {
            string electricityMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await electricityMetersService.CreateElectricityMeterAsync(this.name + i.ToString(), this.powerSupply);
                }

                electricityMeterName = electricityMetersService.GetElectricityMeterById(2).Name;
            }

            Assert.Equal("Test2", electricityMeterName);
        }

        [Fact]
        public async Task TestIfGetElectricityMeterByNameWorksCorrectlyAsync()
        {
            string electricityMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await electricityMetersService.CreateElectricityMeterAsync(this.name + i.ToString(), this.powerSupply);
                }

                electricityMeterName = electricityMetersService.GetElectricityMeterByName("Test2").Name;
            }

            Assert.Equal("Test2", electricityMeterName);
        }

        [Fact]
        public async Task TestIfElectricityMeterIsUpdatedCorrectrlyAsync()
        {
            string electricityMeterName;
            decimal electricityMeterPowerSupply;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                await electricityMetersService.CreateElectricityMeterAsync(this.name, this.powerSupply);
                await electricityMetersService.UpdateElectricityMeterAsync(1, "Updated", 20M);
                electricityMeterName = electricityMetersService.GetElectricityMeterById(1).Name;
                electricityMeterPowerSupply = electricityMetersService.GetElectricityMeterByName(electricityMeterName).PowerSupply;
            }

            Assert.Equal("Updated", electricityMeterName);
            Assert.Equal(20M, electricityMeterPowerSupply);
        }

        [Fact]
        public async Task TestIfElectricityMeterIsDeletedCorrectrlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

            for (int i = 0; i < 3; i++)
            {
                await electricityMetersService.CreateElectricityMeterAsync(this.name + i.ToString(), this.powerSupply);
            }

            await electricityMetersService.DeleteElectricityMeterAsync(1);
            Assert.Equal(2, electricityMetersService.GetAllElectricityMeters().Count());
            Assert.Equal("Test1", electricityMetersService.GetElectricityMeterById(2).Name);
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
