namespace OfficeManager.Tests.TemperatureMetersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using Xunit;

    public class TemperatureMetersServiceTests
    {
        [Fact]
        public async Task TestIfTemperatureMeterIsCreatedCorrectlyAsync()
        {
            int actualTemperatureMetersCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                await temperatureMetersService.CreateTemperatureMeterAsync("TestName1");

                for (int i = 0; i < 3; i++)
                {
                    await temperatureMetersService.CreateTemperatureMeterAsync("TestName2");
                }

                actualTemperatureMetersCount = dbContext.TemperatureMeters.Count();
            }

            Assert.Equal(2, actualTemperatureMetersCount);
        }

        [Fact]
        public async Task TestIfAllTemperatureMetersAreReturnedCorrectrlyAsync()
        {
            string names = string.Empty;
            List<TemperatureMeterOutputViewModel> temperatureMeters = new List<TemperatureMeterOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await temperatureMetersService.CreateTemperatureMeterAsync(i.ToString());
                }

                temperatureMeters = temperatureMetersService.GetAllTemperatureMeters().ToList();
            }

            foreach (var temperatureMeter in temperatureMeters)
            {
                names += temperatureMeter.Name;
            }

            Assert.Equal(3, temperatureMeters.Count);
            Assert.Equal("123", names);
        }

        [Fact]
        public async Task TestIfGetTemperatreMeterByIdWorksCorrectlyAsync()
        {
            string temperatureMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await temperatureMetersService.CreateTemperatureMeterAsync("Test" + i.ToString());
                }

                temperatureMeterName = temperatureMetersService.GetTemperatureMeterById(2).Name;
            }

            Assert.Equal("Test2", temperatureMeterName);
        }

        [Fact]
        public async Task TestIfGetTemperatureMeterByNameWorksCorrectlyAsync()
        {
            string temperatureMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await temperatureMetersService.CreateTemperatureMeterAsync("Test" + i.ToString());
                }

                temperatureMeterName = temperatureMetersService.GetTemperatureMeterByName("Test2").Name;
            }

            Assert.Equal("Test2", temperatureMeterName);
        }

        [Fact]
        public async Task TestIfTemperatureMeterIsUpdatedCorrectrlyAsync()
        {
            string temperatureMeterName;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                await temperatureMetersService.CreateTemperatureMeterAsync("TestName1");
                await temperatureMetersService.UpdateTemperatureMeterAsync(1, "Updated");
                temperatureMeterName = temperatureMetersService.GetTemperatureMeterByName("Updated").Name;
            }

            Assert.Equal("Updated", temperatureMeterName);
        }

        [Fact]
        public async Task TestIfEditTemperatreMeterWorksCorrectlyAsync()
        {
            EditTemperatreMeterViewModel temperatureMeterToEdit = new EditTemperatreMeterViewModel();
            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    await temperatureMetersService.CreateTemperatureMeterAsync("Test" + i.ToString());
                }

                temperatureMeterToEdit = temperatureMetersService.EditTemperatureMeter(2);
            }

            Assert.IsType<EditTemperatreMeterViewModel>(temperatureMeterToEdit);
            Assert.Equal("Test2", temperatureMeterToEdit.Name);
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
