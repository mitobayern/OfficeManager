﻿namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;
        private readonly ITemperatureMetersService temperatureMetersService;
        private readonly IElectricityMetersService electricityMetersService;

        public OfficesController(ApplicationDbContext dbContext,
                                 IOfficesService officesService,
                                 ITemperatureMetersService temperatureMetersService,
                                 IElectricityMetersService electricityMetersService)
        {
            this.dbContext = dbContext;
            this.officesService = officesService;
            this.temperatureMetersService = temperatureMetersService;
            this.electricityMetersService = electricityMetersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateOfficeViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            officesService.CreateOffice(input);

            return Redirect("/Administration/Offices/All");
        }

        public IActionResult All()
        {
            var allOffices = officesService.GetAllOffices().ToList();

            return View(new AllOfficesViewModel { Offices = allOffices });
        }

        public IActionResult Edit(OfficeIdViewModel input)
        {
            var officeToEdit = officesService.EditOffice(input.Id);

            return View(officeToEdit);
        }

        [HttpPost]
        public IActionResult Edit(EditOfficeViewModel input)
        {
            if (!ModelState.IsValid)
            {
                var currentOffice = officesService.EditOffice(input.Id);

                return View(currentOffice);
            }
            officesService.UpdateOffice(input.Id, input.Name, input.Area, input.RentPerSqMeter);

            return Redirect("/Administration/Offices/All");
        }

        public IActionResult AddTemperatureMeters(OfficeIdViewModel input)
        {
            var allAvailableTemperatureMeters = temperatureMetersService
                .GetAllTemperatureMeters()
                .Where(x => x.OfficeNumber == null)
                .ToList();

            return View(new OfficeWithAllTemperatureMetersViewModel { Id = input.Id, AvailavleTemperatureMeters = allAvailableTemperatureMeters });
        }

        [HttpPost]
        public IActionResult AddTemperatureMeters(AddRemoveTemperatureMetersViewModel input)
        {
            officesService.AddTemperatureMetersToOffice(input.Id, input.AreChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult RemoveTemperatureMeters(OfficeIdViewModel input)
        {
            var currentOfficeTemperatreMeters = officesService.GetOfficeTemperatureMeters(input.Id).ToList();


            return View(new OfficeWithCurrentTemperatureMetersViewModel { Id = input.Id, CurrentTemperatureMeters = currentOfficeTemperatreMeters });
        }

        [HttpPost]
        public IActionResult RemoveTemperatureMeters(AddRemoveTemperatureMetersViewModel input)
        {
            officesService.RemoveTemperatureMetersFromOffice(input.Id, input.AreChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult AddElectricityMeter(OfficeIdViewModel input)
        {
            var office = dbContext.Offices.FirstOrDefault(x => x.Id == input.Id);

            var elMeters = dbContext.ElectricityMeters
                .Where(x => x.Office == null)
                .Select(x => new ElectricityMeterOutputViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    OfficeNumber = x.Office.Name,
                    PowerSupply = x.PowerSupply,
                }).ToList();
            ;
            return View(new OfficeWithAllElectricityMetersViewModel { Id = input.Id, AvailavleElectricityMeters = elMeters });
        }

        [HttpPost]
        public IActionResult AddElectricityMeter(AddRemoveElectricityMeterViewModel input)
        {
            officesService.AddElectricityMeterToOffice(input.Id, input.IsChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult RemoveElectricityMeter(OfficeIdViewModel input)
        {
            var currentOffice = officesService.GetOfficeById(input.Id);
            var currentElMeter = currentOffice.ElectricityMeter;

            currentElMeter.Office = null;
            currentElMeter.OfficeId = null;
            currentOffice.ElectricityMeter = null;

            dbContext.SaveChanges();

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }
    }
}
