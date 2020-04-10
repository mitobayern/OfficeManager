namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using Microsoft.EntityFrameworkCore;
    using System;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TemperatureMetersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITemperatureMetersService temperatureMetersService;
        private const string officesAscending = "offices_asc";
        private const string officesDescending = "offices_desc";
        private const string temperatureMetersAscending = "meters_asc";
        private const string temperatureMetersDescending = "meters_desc";

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

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var allTemperatureMeters = temperatureMetersService.GetAllTemperatureMeters();

            allTemperatureMeters = OrderTemperatureMetersAsync(sortOrder, currentFilter, searchString, pageNumber, allTemperatureMeters);



            int pageSize = 5;
            return View(await PaginatedList<TemperatureMeterOutputViewModel>.CreateAsync(allTemperatureMeters.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        private IQueryable<TemperatureMeterOutputViewModel> OrderTemperatureMetersAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<TemperatureMeterOutputViewModel> allTemperatureMeters)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["order"] = "Company: Z to A";
            ViewData["TemperatureMeterSortParam"] = String.IsNullOrEmpty(sortOrder) ? temperatureMetersDescending : "";
            ViewData["OfficeNameSortParm"] = sortOrder == officesAscending ? officesDescending : officesAscending;
            

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                allTemperatureMeters = allTemperatureMeters.Where(s => s.Name.Contains(searchString)
                                       || s.OfficeNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case temperatureMetersDescending:
                    allTemperatureMeters = allTemperatureMeters.OrderByDescending(s => s.Name);
                    ViewData["order"] = temperatureMetersDescending;
                    break;
                case officesAscending:
                    allTemperatureMeters = allTemperatureMeters.OrderBy(s => s.OfficeNumber);
                    ViewData["order"] = officesAscending;
                    break;
                case officesDescending:
                    allTemperatureMeters = allTemperatureMeters.OrderByDescending(s => s.OfficeNumber);
                    ViewData["order"] = officesDescending;
                    break;
                default:
                    allTemperatureMeters = allTemperatureMeters.OrderBy(s => s.Name);
                    ViewData["order"] = temperatureMetersAscending;
                    break;
            }
            return allTemperatureMeters;
        }

        //public IActionResult All()
        //{
        //    var allTemperatureMeters = temperatureMetersService.GetAllTemperatureMeters().ToList();

        //    return View(new AllTemperatureMetersViewModel { TemperatureMeters = allTemperatureMeters });
        //}


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
