namespace OfficeManager.Services
{
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;

    public interface IPricesInformationService
    {
        void CreatePricelist(CreatePricesInputViewModel input);

        CurrentPricesOutputViewModel GetCurrentPrices();

        CurrentPricesOutputViewModel GetPricesInformationById(int id);
    }
}
