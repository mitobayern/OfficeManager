namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TemperatureMetersController : Controller
    {
        private readonly ITemperatureMetersService temperatureMetersService;

        public TemperatureMetersController(ITemperatureMetersService temperatureMetersService)
        {
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
    }
}
