namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TemperatureMetersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITemperatureMetersService temperatureMetersService;

        public TemperatureMetersController(ApplicationDbContext dbContext, ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.temperatureMetersService = temperatureMetersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTemperatureMeterViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            temperatureMetersService.CreateTemperatureMeter(input);

            return Redirect("/Administration/TemperatureMeters/All");
        }

        public IActionResult All()
        {
            var allTemperatureMeters = temperatureMetersService.GetAllTemperatureMeters().ToList();

            return View(new AllTemperatureMetersViewModel { TemperatureMeters = allTemperatureMeters });
        }


        public IActionResult Edit(TemperatureMeterIdViewModel input)
        {
            if (!ValidateTemperatureMeter(input.Id))
            {
                return this.Redirect("/Administration/TemperatureMeters/All");
            }

            var temperatureMeterToEdit = temperatureMetersService.EditTemperatureMeter(input.Id);

            return View(temperatureMeterToEdit);
        }

        [HttpPost]
        public IActionResult Edit(EditTemperatreMeterViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            temperatureMetersService.UpdateTemperatureMeter(input);

            return Redirect("/Administration/TemperatureMeters/All");
        }

        private bool ValidateTemperatureMeter(int id)
        {
            if (this.dbContext.TemperatureMeters.Any(x => x.Id == id))
            {
                return true;
            }
            return false;
        }

    }
}
