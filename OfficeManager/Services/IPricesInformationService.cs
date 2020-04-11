namespace OfficeManager.Services
{
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;

    public interface IPricesInformationService
    {
        Task CreatePricelistAsync(CreatePricesInputViewModel input);

        CurrentPricesOutputViewModel GetCurrentPrices();

        CurrentPricesOutputViewModel GetPricesInformationById(int id);
    }
}
