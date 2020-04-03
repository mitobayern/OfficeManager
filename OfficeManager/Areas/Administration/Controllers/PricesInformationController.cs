namespace OfficeManager.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;

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
            return View();
        }

        [HttpPost]
        public IActionResult CreatePricelist(CreatePricesInputViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            pricesInformationService.CreatePricelist(input);

            return Redirect("/Home/Index");
        }

        public IActionResult CurrentPrices()
        {
            var currentPrices = pricesInformationService.GetCurrentPrices();

            return View(currentPrices);
        }
    }
}
