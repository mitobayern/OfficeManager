using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
using OfficeManager.Data;
using OfficeManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.PricesInformationTests
{
    public class PricesInformationServiceTests
    {
        [Fact]
        public void TestIfPriceListIsCreatedCorrectly()
        {
            int actualPricelistCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    pricesInformationService.CreatePricelist(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = 0.5M,
                        HeatingPerKWh = 1.5M,
                        CoolingPerKWh = 2.0M,
                        AccessToDistributionGrid = 0.2M,
                        NetworkTaxesAndUtilities = 0.2M,
                        Excise = 0.2M,
                    });
                }
                actualPricelistCount = dbContext.PricesInformation.Count();
            }

            Assert.Equal(3, actualPricelistCount);
        }

        [Fact]
        public void TestIfGetCurentPricelistReturnsCorrectly()
        {
            decimal currentElectricityPerKWhPrice;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    pricesInformationService.CreatePricelist(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = i + 0.5M,
                        HeatingPerKWh = 1.5M,
                        CoolingPerKWh = 2.0M,
                        AccessToDistributionGrid = 0.2M,
                        NetworkTaxesAndUtilities = 0.2M,
                        Excise = 0.2M,
                    });
                }
                currentElectricityPerKWhPrice = pricesInformationService.GetCurrentPrices().ElectricityPerKWh;
            }

            Assert.Equal(2.5M, currentElectricityPerKWhPrice);
        }

        [Fact]
        public void TestIfGetPricelistByIdReturnsCorrectly()
        {
            decimal currentElectricityPerKWhPrice;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    pricesInformationService.CreatePricelist(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = i + 0.5M,
                        HeatingPerKWh = 1.5M,
                        CoolingPerKWh = 2.0M,
                        AccessToDistributionGrid = 0.2M,
                        NetworkTaxesAndUtilities = 0.2M,
                        Excise = 0.2M,
                    });
                }
                currentElectricityPerKWhPrice = pricesInformationService.GetPricesInformationById(2).ElectricityPerKWh;
            }

            Assert.Equal(1.5M, currentElectricityPerKWhPrice);
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
