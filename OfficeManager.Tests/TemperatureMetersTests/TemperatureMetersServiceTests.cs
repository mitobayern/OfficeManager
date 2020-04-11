using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Data;
using OfficeManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.TemperatureMetersTests
{
    public class TemperatureMetersServiceTests
    {
        [Fact]
        public void TestIfTemperatureMeterIsCreatedCorrectly()
        {
            int actualTemperatureMetersCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                {
                    Name = "TestName1"                    
                });

                for (int i = 0; i < 3; i++)
                {
                    temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = "TestName2"
                    });
                }

                actualTemperatureMetersCount = dbContext.TemperatureMeters.Count();
            }

            Assert.Equal(2, actualTemperatureMetersCount);
        }

        [Fact]
        public void TestIfAllTemperatureMetersAreReturnedCorrectrly()
        {
            string names = string.Empty;
            List<TemperatureMeterOutputViewModel> temperatureMeters = new List<TemperatureMeterOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = i.ToString(),
                    });
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
        public void TestIfGetTemperatreMeterByIdWorksCorrectly()
        {
            string temperatureMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = "Test" + i.ToString(),
                    });
                }

                temperatureMeterName = temperatureMetersService.GetTemperatureMeterById(2).Name;
            }

            Assert.Equal("Test2", temperatureMeterName);
        }

        [Fact]
        public void TestIfGetTemperatureMeterByNameWorksCorrectly()
        {
            string temperatureMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = "Test" + i.ToString()
                    });
                }

                temperatureMeterName = temperatureMetersService.GetTemperatureMeterByName("Test2").Name;
            }

            Assert.Equal("Test2", temperatureMeterName);
        }

        [Fact]
        public void TestIfTemperatureMeterIsUpdatedCorrectrly()
        {
            string temperatureMeterName;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                {
                    Name = "TestName1"
                });

                EditTemperatreMeterViewModel temperatureMeterToUpdate = new EditTemperatreMeterViewModel
                {
                    Id = 1,
                    Name = "Updated"
                };

                temperatureMetersService.UpdateTemperatureMeterAsync(temperatureMeterToUpdate);
                temperatureMeterName = temperatureMetersService.GetTemperatureMeterByName("Updated").Name;
            }

            Assert.Equal("Updated", temperatureMeterName);
        }

        [Fact]
        public void TestIfEditTemperatreMeterWorksCorrectly()
        {
            EditTemperatreMeterViewModel temperatureMeterToEdit = new EditTemperatreMeterViewModel();
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMetersService = new TemperatureMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    temperatureMetersService.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = "Test" + i.ToString(),
                    });
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
