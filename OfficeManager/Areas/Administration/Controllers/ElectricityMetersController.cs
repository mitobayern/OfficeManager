namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ElectricityMetersController : Controller
    {
        private readonly IElectricityMetersService electricityMetersService;

        public ElectricityMetersController(IElectricityMetersService electricityMetersService)
        {
            this.electricityMetersService = electricityMetersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateElectricityMeterViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            electricityMetersService.CreateElectricityMeter(input);

            return Redirect("/Administration/ElectricityMeters/All");
        }

        public IActionResult All()
        {
            var allElectricityMeters = electricityMetersService.GetAllElectricityMeters().ToList();

            return View(new AllElectricityMetersViewModel { ElectricityMeters = allElectricityMeters });
        }

        public IActionResult Edit(ElectricityMeterIdViewModel input)
        {
            var electricityMeterToEdit = electricityMetersService.GetElectricityMeterById(input.Id);
            var electricityMeter = new ElectricityMeterOutputViewModel
            {
                Id = electricityMeterToEdit.Id,
                Name = electricityMeterToEdit.Name,
                PowerSupply = electricityMeterToEdit.PowerSupply,
            };

            return View(electricityMeter);
        }

        [HttpPost]
        public IActionResult Edit(ElectricityMeterOutputViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            electricityMetersService.UpdateElectricityMeter(input);

            return Redirect("/Administration/ElectricityMeters/All");
        }
    }
}
