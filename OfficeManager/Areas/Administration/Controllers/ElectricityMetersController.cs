namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Data;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ElectricityMetersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IElectricityMetersService electricityMetersService;
        private const string officesAscending = "offices_asc";
        private const string officesDescending = "offices_desc";
        private const string powerSupplyAscending = "power_asc";
        private const string powerSuplpyDescending = "power_desc";
        private const string electricityMetersAscending = "meters_asc";
        private const string electricityMetersDescending = "meters_desc";

        public ElectricityMetersController(ApplicationDbContext dbContext, IElectricityMetersService electricityMetersService)
        {
            this.dbContext = dbContext;
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

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allElectricityMeters = electricityMetersService.GetAllElectricityMeters();

            allElectricityMeters = OrderElectricityMetersAsync(sortOrder, currentFilter, searchString, pageNumber, allElectricityMeters);
            
            int pageSize;

            if (String.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allElectricityMeters.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            ViewData["RowsPerPage"] = pageSize;

            return View(await PaginatedList<ElectricityMeterOutputViewModel>.CreateAsync(allElectricityMeters.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Edit(ElectricityMeterIdViewModel input)
        {
            if (!ValidateElectricityMeter(input.Id))
            {
                return this.Redirect("/Administration/ElectricityMeters/All");
            }

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

        private IQueryable<ElectricityMeterOutputViewModel> OrderElectricityMetersAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<ElectricityMeterOutputViewModel> allElectricityMeters)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["order"] = "Company: Z to A";
            ViewData["ElectricityMeterSortParam"] = String.IsNullOrEmpty(sortOrder) ? electricityMetersDescending : "";
            ViewData["OfficeNameSortParm"] = sortOrder == officesAscending ? officesDescending : officesAscending;
            ViewData["PowerSupplySortParm"] = sortOrder == powerSupplyAscending ? powerSuplpyDescending : powerSupplyAscending;
            
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
                allElectricityMeters = allElectricityMeters.Where(s => s.Name.Contains(searchString)
                                       || s.OfficeNumber.Contains(searchString));
            }

            switch (sortOrder)
            {
                case electricityMetersDescending:
                    allElectricityMeters = allElectricityMeters.OrderByDescending(s => s.Name);
                    ViewData["order"] = electricityMetersDescending;
                    break;
                case officesAscending:
                    allElectricityMeters = allElectricityMeters.OrderBy(s => s.OfficeNumber);
                    ViewData["order"] = officesAscending;
                    break;
                case officesDescending:
                    allElectricityMeters = allElectricityMeters.OrderByDescending(s => s.OfficeNumber);
                    ViewData["order"] = officesDescending;
                    break;
                case powerSupplyAscending:
                    allElectricityMeters = allElectricityMeters.OrderBy(s => s.OfficeNumber);
                    ViewData["order"] = powerSupplyAscending;
                    break;
                case powerSuplpyDescending:
                    allElectricityMeters = allElectricityMeters.OrderByDescending(s => s.PowerSupply);
                    ViewData["order"] = powerSuplpyDescending;
                    break;
                default:
                    allElectricityMeters = allElectricityMeters.OrderBy(s => s.PowerSupply);
                    ViewData["order"] = electricityMetersAscending;
                    break;
            }
            return allElectricityMeters;
        }

        private bool ValidateElectricityMeter(int id)
        {
            if (this.dbContext.ElectricityMeters.Any(x => x.Id == id))
            {
                return true;
            }
            return false;
        }
    }
}
