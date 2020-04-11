namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class PricesInformationController : Controller
    {
        private readonly IPricesInformationService pricesInformationService;

        public PricesInformationController(IPricesInformationService pricesInformationService)
        {
            this.pricesInformationService = pricesInformationService;
        }

        public IActionResult CreatePricelist()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePricelistAsync(CreatePricesInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.pricesInformationService.CreatePricelistAsync(input);

            return this.Redirect("/Administration/PricesInformation/CurrentPrices");
        }

        public IActionResult CurrentPrices()
        {
            var currentPrices = this.pricesInformationService.GetCurrentPrices();

            return this.View(currentPrices);
        }
    }
}
