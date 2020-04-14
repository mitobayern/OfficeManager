namespace OfficeManager.Tests.PricesInformationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using Xunit;

    public class PricesInformationServiceTests
    {
        private readonly decimal heatingPerKWh;
        private readonly decimal coolingPerKWh;
        private readonly decimal accessToDistributionGrid;
        private readonly decimal networkTaxesAndUtilities;
        private readonly decimal excise;

        public PricesInformationServiceTests()
        {
            this.heatingPerKWh = 1.5M;
            this.coolingPerKWh = 2.0M;
            this.accessToDistributionGrid = 0.2M;
            this.networkTaxesAndUtilities = 0.2M;
            this.excise = 0.2M;
        }

        [Fact]
        public async Task TestIfPriceListIsCreatedCorrectlyAsync()
        {
            int actualPricelistCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    await pricesInformationService.CreatePricelistAsync(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = 0.5M,
                        HeatingPerKWh = this.heatingPerKWh,
                        CoolingPerKWh = this.coolingPerKWh,
                        AccessToDistributionGrid = this.accessToDistributionGrid,
                        NetworkTaxesAndUtilities = this.networkTaxesAndUtilities,
                        Excise = this.excise,
                    });
                }

                actualPricelistCount = dbContext.PricesInformation.Count();
            }

            Assert.Equal(3, actualPricelistCount);
        }

        [Fact]
        public async Task TestIfGetCurentPricelistReturnsCorrectlyAsync()
        {
            decimal currentElectricityPerKWhPrice;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    await pricesInformationService.CreatePricelistAsync(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = i + 0.5M,
                        HeatingPerKWh = this.heatingPerKWh,
                        CoolingPerKWh = this.coolingPerKWh,
                        AccessToDistributionGrid = this.accessToDistributionGrid,
                        NetworkTaxesAndUtilities = this.networkTaxesAndUtilities,
                        Excise = this.excise,
                    });
                }

                currentElectricityPerKWhPrice = pricesInformationService.GetCurrentPrices().ElectricityPerKWh;
            }

            Assert.Equal(2.5M, currentElectricityPerKWhPrice);
        }

        [Fact]
        public async Task TestIfGetPricelistByIdReturnsCorrectlyAsync()
        {
            decimal currentElectricityPerKWhPrice;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                IPricesInformationService pricesInformationService = new PricesInformationService(dbContext);

                for (int i = 0; i < 3; i++)
                {
                    await pricesInformationService.CreatePricelistAsync(new CreatePricesInputViewModel
                    {
                        ElectricityPerKWh = i + 0.5M,
                        HeatingPerKWh = this.heatingPerKWh,
                        CoolingPerKWh = this.coolingPerKWh,
                        AccessToDistributionGrid = this.accessToDistributionGrid,
                        NetworkTaxesAndUtilities = this.networkTaxesAndUtilities,
                        Excise = this.excise,
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
