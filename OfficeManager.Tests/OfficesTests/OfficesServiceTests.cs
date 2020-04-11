using Microsoft.EntityFrameworkCore;
using Moq;
using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Data;
using OfficeManager.Models;
using OfficeManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.OfficesTests
{
    public class OfficesServiceTests
    {
        private CreateTenantViewModel inputTenant;
        private CreateOfficeViewModel inputOffice;
        private Mock<ITenantsService> tenantsService;
        private Mock<IElectricityMetersService> electricityMetersService;
        private Mock<ITemperatureMetersService> temperatureMetersService;

        public OfficesServiceTests()
        {
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
            this.inputOffice = new CreateOfficeViewModel
            {
                Name = "TestOfficeName",
                Area = 50M,
                RentPerSqMeter = 7.2M
            };
            tenantsService = new Mock<ITenantsService>();
            electricityMetersService = new Mock<IElectricityMetersService>();
            temperatureMetersService = new Mock<ITemperatureMetersService>();
        }

        [Fact]
        public void TestIfOfficeIsCreatedCorrectly()
        {
            int actualOfficesCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);

                officesService.CreateOfficeAsync(new CreateOfficeViewModel
                {
                    Name = "FirstTestOfficeName",
                    Area = 50M,
                    RentPerSqMeter = 7.2M
                });

                for (int i = 0; i < 3; i++)
                {
                    officesService.CreateOfficeAsync(new CreateOfficeViewModel
                    {
                        Name = "SecondTestOfficeName",
                        Area = 50M,
                        RentPerSqMeter = 7.2M
                    });
                }

                actualOfficesCount = dbContext.Offices.Count();
            }

            Assert.Equal(2, actualOfficesCount);
        }

        [Fact]
        public void TestIfOfficeIsUpdatedCorrectly()
        {            
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);

                officesService.CreateOfficeAsync(inputOffice);
                officesService.UpdateOfficeAsync(1, "UpdatedOffice", 20M, 10M);

                Assert.Equal("UpdatedOffice", officesService.GetOfficeById(1).Name);
                Assert.Equal(20M, officesService.GetOfficeByName("UpdatedOffice").Area);
                Assert.Equal(10M, officesService.GetOfficeByName("UpdatedOffice").RentPerSqMeter);
            }
        }

        [Fact]
        public void TestIfOfficeIsReturnedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);
                officesService.CreateOfficeAsync(inputOffice);

                Assert.Equal("No electricity meter available", officesService.EditOffice(1).ElectricityMeter);
                Assert.Equal(50M, officesService.EditOffice(1).Area);
                Assert.Equal(7.2M, officesService.EditOffice(1).RentPerSqMeter);
            }
        }

        [Fact]
        public void TestIfGetOfficeByIdReturnsCorrectrly()
        {
            string officeName;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);

                officesService.CreateOfficeAsync(inputOffice);

                officeName = officesService.GetOfficeById(1).Name;
            }

            Assert.Equal("TestOfficeName", officeName);
        }

        [Fact]
        public void TestIfGetOfficeByNameReturnsCorrectrly()
        {
            string officeName;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);

                officesService.CreateOfficeAsync(inputOffice);

                officeName = officesService.GetOfficeByName("TestOfficeName").Name;
            }

            Assert.Equal("TestOfficeName", officeName);
        }

        [Fact]
        public void TestIfOfficesAreAddedAndRemovedToTenantCorectly()
        {
            int tenantOfficesCount;
            List<string> officesToAdd = new List<string> { "1", "3", "5" };
            List<string> officesToRemove = new List<string> { "3" };

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenants =
                   new TenantsService(dbContext);

                IOfficesService officesService =
                    new OfficesService(dbContext, tenants, electricityMetersService.Object, temperatureMetersService.Object);

                AddOfficesToTenant(inputTenant, tenants, officesService);

                officesService.AddOfficesToTenantAsync(1, officesToAdd);
                officesService.RemoveOfficesFromTenantAsync(1, officesToRemove);
                tenantOfficesCount = dbContext.Tenants.FirstOrDefault(x => x.CompanyName == "TestCompanyName").Offices.Count();
            }

            Assert.Equal(2, tenantOfficesCount);
        }

        [Fact]
        public void TestIfAvailableOfficesAreReturnedCorectly()
        {
            int availableOfficesCount;
            List<string> offices = new List<string> { "1", "3" };
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenants =
                  new TenantsService(dbContext);

                IOfficesService officesService =
                    new OfficesService(dbContext, tenants, electricityMetersService.Object, temperatureMetersService.Object);

                AddOfficesToTenant(inputTenant, tenants, officesService);

                officesService.AddOfficesToTenantAsync(1, offices);
                availableOfficesCount = officesService.GetAllAvailableOffices().Count();
            }

            Assert.Equal(1, availableOfficesCount);
        }

        [Fact]
        public void TestIfAllOfficesAreReturnedCorrectrly()
        {
            string names = string.Empty;
            List<OfficeOutputViewModel> allOffices = new List<OfficeOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMetersService.Object);

                for (int i = 1; i <= 3; i++)
                {
                    officesService.CreateOfficeAsync(new CreateOfficeViewModel
                    {
                        Name = i.ToString(),
                        Area = 50M,
                        RentPerSqMeter = 7.2M
                    });
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
        public void TestIfElectricityMeterIsAddedAndRemovedToOfficeCorrectly()
        {
            string actualElectricityMetersName;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMeters =
                    new ElectricityMetersService(dbContext);

                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMeters, temperatureMetersService.Object);

                officesService.CreateOfficeAsync(inputOffice);

                electricityMeters.CreateElectricityMeter(new CreateElectricityMeterViewModel
                {
                    Name = "TestElectricityMeter",
                    PowerSupply = 5M,
                });

                officesService.AddElectricityMeterToOfficeAsync(1, "TestElectricityMeter");

                actualElectricityMetersName = officesService.GetOfficeByName("TestOfficeName").ElectricityMeter.Name;

                officesService.RemoveElectricityMeterFromOfficeAsync(1);
                Assert.Throws<NullReferenceException>(() => officesService.GetOfficeByName("TestOfficeName").ElectricityMeter.Name);
            }
        }

        [Fact]
        public void TestIfTemperatureMetersAreAddedAndRemovedToOfficeCorrectly()
        {
            int temperatureMetersCount;
            List<string> temperatureMetersToAdd = new List<string> { "1", "3", "5" };
            List<string> temperatureMetersToRemove = new List<string> { "3" };
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters =
                    new TemperatureMetersService(dbContext);

                IOfficesService officesService =
                    new OfficesService(dbContext, tenantsService.Object, electricityMetersService.Object, temperatureMeters);

                officesService.CreateOfficeAsync(inputOffice);

                for (int i = 1; i <= 5; i++)
                {
                    temperatureMeters.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                    {
                        Name = i.ToString()
                    });
                }

                var office = officesService.GetOfficeById(1);
                var count = dbContext.TemperatureMeters.Count();
                officesService.AddTemperatureMetersToOfficeAsync(1, temperatureMetersToAdd);
                officesService.RemoveTemperatureMetersFromOfficeAsync(1, temperatureMetersToRemove);
                temperatureMetersCount = officesService.GetOfficeTemperatureMeters(1).ToList().Count();
            }
            Assert.Equal(2, temperatureMetersCount);
        }

        private static void AddOfficesToTenant(CreateTenantViewModel inputTenant, ITenantsService tenants, IOfficesService officesService)
        {
            tenants.CreateTenantAsync(inputTenant);

            for (int i = 1; i <= 5; i++)
            {
                officesService.CreateOfficeAsync(new CreateOfficeViewModel
                {
                    Name = i.ToString(),
                    Area = 50M,
                    RentPerSqMeter = 7.2M
                });
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
