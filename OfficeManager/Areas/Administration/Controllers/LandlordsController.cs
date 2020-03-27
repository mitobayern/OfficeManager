namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;

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
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateLandlordViewModel input)
        {
            if (dbContext.Landlords.Count() == 1)
            {
                return Redirect("/Administration/Landlords/Details");
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }
            landlordsService.CreateLandlord(input);

            return Redirect("/Administration/Landlords/Details");
        }

        public IActionResult Details()
        {
            var landlord = landlordsService.GetLandlord();

            return View(landlord);
        }

        public IActionResult Edit()
        {
            var landlord = landlordsService.EditLandlord();

            return View(landlord);
        }

        [HttpPost]
        public IActionResult Edit(CreateLandlordViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            landlordsService.UpdateLandlord(input);

            return Redirect("/Administration/Landlords/Details");
        }
    }
}
