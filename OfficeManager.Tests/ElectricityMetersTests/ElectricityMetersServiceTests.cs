using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Data;
using OfficeManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.ElectricityMetersTests
{
    public class ElectricityMetersServiceTests
    {
        [Fact]
        public void TestIfElectricityMeterIsCreatedCorrectly()
        {
            int actualElectricityMetersCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                {
                    Name = "TestName1",
                    PowerSupply = 10M,
                });

                for (int i = 0; i < 3; i++)
                {
                    electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                    {
                        Name = "TestName2",
                        PowerSupply = 10M,
                    });
                }

                actualElectricityMetersCount = dbContext.ElectricityMeters.Count();
            }

            Assert.Equal(2, actualElectricityMetersCount);
        }

        [Fact]
        public void TestIfAllElectricityMetersAreReturnedCorrectrly()
        {
            string names = string.Empty;
            List<ElectricityMeterOutputViewModel> electricityMeters = new List<ElectricityMeterOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                    {
                        Name = i.ToString(),
                        PowerSupply = 10M,
                    });
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
        public void TestIfGetElectricityMeterByIdWorksCorrectly()
        {
            string electricityMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                    {
                        Name = "Test" + i.ToString(),
                        PowerSupply = 10M,
                    });
                }

                electricityMeterName = electricityMetersService.GetElectricityMeterById(2).Name;
            }

            Assert.Equal("Test2", electricityMeterName);
        }

        [Fact]
        public void TestIfGetElectricityMeterByNameWorksCorrectly()
        {
            string electricityMeterName = string.Empty;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                    {
                        Name = "Test" + i.ToString(),
                        PowerSupply = 10M,
                    });
                }

                electricityMeterName = electricityMetersService.GetElectricityMeterByName("Test2").Name;
            }

            Assert.Equal("Test2", electricityMeterName);
        }

        [Fact]
        public void TestIfElectricityMeterIsUpdatedCorrectrly()
        {
            string electricityMeterName;
            decimal electricityMeterPowerSupply;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMetersService = new ElectricityMetersService(dbContext);

                electricityMetersService.CreateElectricityMeter(new CreateElectricityMeterViewModel
                {
                    Name = "Test",
                    PowerSupply = 10M,
                });

                ElectricityMeterOutputViewModel electricityMeterToUpdate = new ElectricityMeterOutputViewModel
                {
                    Id = 1,
                    Name = "Updated",
                    PowerSupply = 20M,
                };
                electricityMetersService.UpdateElectricityMeter(electricityMeterToUpdate);
                electricityMeterName = electricityMetersService.GetElectricityMeterById(1).Name;
                electricityMeterPowerSupply = electricityMetersService.GetElectricityMeterByName(electricityMeterName).PowerSupply;
            }

            Assert.Equal("Updated", electricityMeterName);
            Assert.Equal(20M, electricityMeterPowerSupply);
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
