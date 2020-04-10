namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using System;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;
        private readonly ITemperatureMetersService temperatureMetersService;
        private readonly IElectricityMetersService electricityMetersService;
        private const string officesAscending = "office_asc";
        private const string officesDescending = "office_desc";
        private const string tenantsAscending = "tenant_asc";
        private const string tenantsDescending = "tenant_desc";
        private const string areaAscending = "area_asc";
        private const string areaDescending = "area_desc";
        private const string rentAscending = "rent_asc";
        private const string rentDescending = "rent_desc";



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

        public IActionResult All(string sortOrder)
        {
            ViewData["order"] = "Company: Z to A";
            ViewData["OfficeNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? officesDescending : "";
            ViewData["TenantSortParam"] = sortOrder == tenantsAscending ? tenantsDescending : tenantsAscending;
            ViewData["AreaSortParam"] = sortOrder == areaAscending ? areaDescending : areaAscending;
            ViewData["RentSortParam"] = sortOrder == rentAscending ? rentDescending : rentAscending;



            var allOffices = officesService.GetAllOffices().ToList();

            switch (sortOrder)
            {
                case officesDescending:
                    allOffices = allOffices.OrderByDescending(s => s.Name).ToList();
                    ViewData["order"] = officesDescending;
                    break;
                case tenantsAscending:
                    allOffices = allOffices.OrderBy(s => s.TenantName).ToList();
                    ViewData["order"] = tenantsAscending;
                    break;
                case tenantsDescending:
                    allOffices = allOffices.OrderByDescending(s => s.TenantName).ToList();
                    ViewData["order"] = tenantsDescending;
                    break;
                case areaAscending:
                    allOffices = allOffices.OrderBy(s => s.Area).ToList();
                    ViewData["order"] = areaAscending;
                    break;
                case areaDescending:
                    allOffices = allOffices.OrderByDescending(s => s.Area).ToList();
                    ViewData["order"] = areaDescending;
                    break;
                case rentAscending:
                    allOffices = allOffices.OrderBy(s => s.RentPerSqMeter).ToList();
                    ViewData["order"] = rentAscending;
                    break;
                case rentDescending:
                    allOffices = allOffices.OrderByDescending(s => s.RentPerSqMeter).ToList();
                    ViewData["order"] = rentDescending;
                    break;
                default:
                    allOffices = allOffices.OrderBy(s => s.Name).ToList();
                    ViewData["order"] = officesAscending;
                    break;
            }

            return View(new AllOfficesViewModel { Offices = allOffices });
        }

        public IActionResult Edit(OfficeIdViewModel input)
        {
            if (!ValidateOffice(input.Id))
            {
                return this.Redirect("/Administration/Offices/All");
            }
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
            if (!ValidateOffice(input.Id))
            {
                return this.Redirect("/Administration/Offices/All");
            }

            var allAvailableTemperatureMeters = temperatureMetersService
                .GetAllTemperatureMeters()
                .Where(x => x.OfficeNumber == null)
                .ToList();

            return View(new OfficeWithAllTemperatureMetersViewModel { Id = input.Id, AvailavleTemperatureMeters = allAvailableTemperatureMeters });
        }

        [HttpPost]
        public IActionResult AddTemperatureMeters(AddRemoveTemperatureMetersViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("AddTemperatureMeters", new OfficeIdViewModel { Id = input.Id });
            }
            officesService.AddTemperatureMetersToOffice(input.Id, input.AreChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult RemoveTemperatureMeters(OfficeIdViewModel input)
        {
            if (!ValidateOffice(input.Id))
            {
                return this.Redirect("/Administration/Offices/All");
            }

            var currentOfficeTemperatreMeters = officesService.GetOfficeTemperatureMeters(input.Id).ToList();
            
            return View(new OfficeWithCurrentTemperatureMetersViewModel { Id = input.Id, CurrentTemperatureMeters = currentOfficeTemperatreMeters });
        }

        [HttpPost]
        public IActionResult RemoveTemperatureMeters(AddRemoveTemperatureMetersViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("RemoveTemperatureMeters", new OfficeIdViewModel { Id = input.Id });
            }

            officesService.RemoveTemperatureMetersFromOffice(input.Id, input.AreChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult AddElectricityMeter(OfficeIdViewModel input)
        {
            if (!ValidateOffice(input.Id))
            {
                return this.Redirect("/Administration/Offices/All");
            }

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
            if (input.IsChecked == null)
            {
                return this.RedirectToAction("AddElectricityMeter", new OfficeIdViewModel { Id = input.Id });
            }

            officesService.AddElectricityMeterToOffice(input.Id, input.IsChecked);

            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult RemoveElectricityMeter(OfficeIdViewModel input)
        {
            if (!ValidateOffice(input.Id))
            {
                return this.Redirect("/Administration/Offices/All");
            }
            
            this.officesService.RemoveElectricityMeterFromOffice(input.Id);


            return Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        private bool ValidateOffice(int id)
        {
            if (this.dbContext.Offices.Any(x => x.Id == id))
            {
                return true;
            }
            return false;
        }
    }
}
