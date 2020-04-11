namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class LandlordsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILandlordsService landlordsService;

        public LandlordsController(ApplicationDbContext dbContext, ILandlordsService landlordsService)
        {
            this.dbContext = dbContext;
            this.landlordsService = landlordsService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLandlordViewModel input)
        {
            if (this.dbContext.Landlords.Count() == 1)
            {
                return this.Redirect("/Administration/Landlords/Details");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.landlordsService.CreateLandlordAsync(input);

            return this.Redirect("/Administration/Landlords/Details");
        }

        public IActionResult Details()
        {
            var landlord = this.landlordsService.GetLandlord();

            return this.View(landlord);
        }

        public IActionResult Edit()
        {
            var landlord = this.landlordsService.GetLandlord();

            return this.View(landlord);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(CreateLandlordViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.landlordsService.UpdateLandlordAsync(input);

            return this.Redirect("/Administration/Landlords/Details");
        }
    }
}
