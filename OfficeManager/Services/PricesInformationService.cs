namespace OfficeManager.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class PricesInformationService : IPricesInformationService
    {
        private readonly ApplicationDbContext dbContext;

        public PricesInformationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreatePricelistAsync(CreatePricesInputViewModel input)
        {
            PricesInformation pricesInformation = new PricesInformation
            {
                CreatedOn = DateTime.UtcNow.Date,
                HeatingPerKWh = input.HeatingPerKWh,
                CoolingPerKWh = input.CoolingPerKWh,
                ElectricityPerKWh = input.ElectricityPerKWh,
                Excise = input.Excise,
                AccessToDistributionGrid = input.AccessToDistributionGrid,
                NetworkTaxesAndUtilities = input.NetworkTaxesAndUtilities,
            };

            await this.dbContext.PricesInformation.AddAsync(pricesInformation);
            await this.dbContext.SaveChangesAsync();
        }

        public CurrentPricesOutputViewModel GetCurrentPrices()
        {
            var currentPrices = this.dbContext.PricesInformation
                .OrderByDescending(x => x.Id)
                .Select(x => new CurrentPricesOutputViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn.ToString("d MMMM yyyy"),
                    HeatingPerKWh = x.HeatingPerKWh,
                    CoolingPerKWh = x.CoolingPerKWh,
                    ElectricityPerKWh = x.ElectricityPerKWh,
                    Excise = x.Excise,
                    AccessToDistributionGrid = x.AccessToDistributionGrid,
                    NetworkTaxesAndUtilities = x.NetworkTaxesAndUtilities,
                })
                .First();

            return currentPrices;
        }

        public CurrentPricesOutputViewModel GetPricesInformationById(int id)
        {
            var currentPrices = this.dbContext.PricesInformation
                .Where(x => x.Id == id)
                .Select(x => new CurrentPricesOutputViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn.ToString("d MMMM yyyy"),
                    HeatingPerKWh = x.HeatingPerKWh,
                    CoolingPerKWh = x.CoolingPerKWh,
                    ElectricityPerKWh = x.ElectricityPerKWh,
                    Excise = x.Excise,
                    AccessToDistributionGrid = x.AccessToDistributionGrid,
                    NetworkTaxesAndUtilities = x.NetworkTaxesAndUtilities,
                })
                .First();

            return currentPrices;
        }
    }
}
