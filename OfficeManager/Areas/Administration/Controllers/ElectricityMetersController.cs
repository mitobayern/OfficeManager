namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ElectricityMetersController : Controller
    {
        private const string OfficesAscending = "offices_asc";
        private const string OfficesDescending = "offices_desc";
        private const string PowerSupplyAscending = "power_asc";
        private const string PowerSuplpyDescending = "power_desc";
        private const string ElectricityMetersDescending = "meters_desc";
        private readonly ApplicationDbContext dbContext;
        private readonly IElectricityMetersService electricityMetersService;

        public ElectricityMetersController(ApplicationDbContext dbContext, IElectricityMetersService electricityMetersService)
        {
            this.dbContext = dbContext;
            this.electricityMetersService = electricityMetersService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateElectricityMeterViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.electricityMetersService.CreateElectricityMeterAsync(input.Name, input.PowerSupply);

            return this.Redirect("/Administration/ElectricityMeters/All");
        }

        public IActionResult Edit(ElectricityMeterIdViewModel input)
        {
            if (!this.ValidateElectricityMeter(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var electricityMeterToEdit = this.electricityMetersService.GetElectricityMeterById(input.Id);
            var electricityMeter = new ElectricityMeterOutputViewModel
            {
                Id = electricityMeterToEdit.Id,
                Name = electricityMeterToEdit.Name,
                PowerSupply = electricityMeterToEdit.PowerSupply,
            };

            return this.View(electricityMeter);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ElectricityMeterOutputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.electricityMetersService.UpdateElectricityMeterAsync(input.Id, input.Name, input.PowerSupply);

            return this.Redirect("/Administration/ElectricityMeters/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allElectricityMeters = this.electricityMetersService.GetAllElectricityMeters();
            allElectricityMeters = this.OrderElectricityMeters(sortOrder, currentFilter, searchString, pageNumber, allElectricityMeters);
            int pageSize = GetPageSize(rowsPerPage, allElectricityMeters);

            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<ElectricityMeterOutputViewModel>.CreateAsync(allElectricityMeters.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private static int GetPageSize(string rowsPerPage, IQueryable<ElectricityMeterOutputViewModel> allElectricityMeters)
        {
            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
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

            return pageSize;
        }

        private IQueryable<ElectricityMeterOutputViewModel> OrderElectricityMeters(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<ElectricityMeterOutputViewModel> allElectricityMeters)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["ElectricityMeterSortParam"] = string.IsNullOrEmpty(sortOrder) ? ElectricityMetersDescending : string.Empty;
            this.ViewData["OfficeNameSortParm"] = sortOrder == OfficesAscending ? OfficesDescending : OfficesAscending;
            this.ViewData["PowerSupplySortParm"] = sortOrder == PowerSupplyAscending ? PowerSuplpyDescending : PowerSupplyAscending;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                allElectricityMeters = allElectricityMeters.Where(s => s.Name.Contains(searchString)
                                       || s.OfficeNumber.Contains(searchString));
            }

            allElectricityMeters = sortOrder switch
            {
                ElectricityMetersDescending => allElectricityMeters.OrderByDescending(s => s.Name),
                OfficesAscending => allElectricityMeters.OrderBy(s => s.OfficeNumber),
                OfficesDescending => allElectricityMeters.OrderByDescending(s => s.OfficeNumber),
                PowerSupplyAscending => allElectricityMeters.OrderBy(s => s.OfficeNumber),
                PowerSuplpyDescending => allElectricityMeters.OrderByDescending(s => s.PowerSupply),
                _ => allElectricityMeters.OrderBy(s => s.PowerSupply),
            };
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
