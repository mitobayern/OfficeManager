namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TemperatureMetersController : Controller
    {
        private const string OfficesAscending = "offices_asc";
        private const string OfficesDescending = "offices_desc";
        private const string TemperatureMetersDescending = "meters_desc";
        private readonly ApplicationDbContext dbContext;
        private readonly ITemperatureMetersService temperatureMetersService;

        public TemperatureMetersController(ApplicationDbContext dbContext, ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.temperatureMetersService = temperatureMetersService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTemperatureMeterViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.temperatureMetersService.CreateTemperatureMeterAsync(input.Name);

            return this.Redirect("/Administration/TemperatureMeters/All");
        }

        public IActionResult Edit(TemperatureMeterIdViewModel input)
        {
            if (!this.ValidateTemperatureMeter(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var temperatureMeterToEdit = this.temperatureMetersService.EditTemperatureMeter(input.Id);

            return this.View(temperatureMeterToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTemperatreMeterViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.temperatureMetersService.UpdateTemperatureMeterAsync(input.Id, input.Name);

            return this.Redirect("/Administration/TemperatureMeters/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allTemperatureMeters = this.temperatureMetersService.GetAllTemperatureMeters();

            allTemperatureMeters = this.OrderTemperatureMetersAsync(sortOrder, currentFilter, searchString, pageNumber, allTemperatureMeters);
            int pageSize = GetPageSize(rowsPerPage, allTemperatureMeters);

            this.ViewData["RowsPerPage"] = pageSize;
            return this.View(await PaginatedList<TemperatureMeterOutputViewModel>.CreateAsync(allTemperatureMeters.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TemperatureMeterIdViewModel input)
        {
            await this.temperatureMetersService.DeleteTemperatureMeterAsync(input.Id);

            return this.Redirect("/Administration/TemperatureMeters/All");
        }

        private static int GetPageSize(string rowsPerPage, IQueryable<TemperatureMeterOutputViewModel> allTemperatureMeters)
        {
            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allTemperatureMeters.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            return pageSize;
        }

        private IQueryable<TemperatureMeterOutputViewModel> OrderTemperatureMetersAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<TemperatureMeterOutputViewModel> allTemperatureMeters)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["TemperatureMeterSortParam"] = string.IsNullOrEmpty(sortOrder) ? TemperatureMetersDescending : string.Empty;
            this.ViewData["OfficeNameSortParm"] = sortOrder == OfficesAscending ? OfficesDescending : OfficesAscending;

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
                allTemperatureMeters = allTemperatureMeters.Where(s => s.Name.Contains(searchString)
                                       || s.OfficeNumber.Contains(searchString));
            }

            allTemperatureMeters = sortOrder switch
            {
                TemperatureMetersDescending => allTemperatureMeters.OrderByDescending(s => s.Name),
                OfficesAscending => allTemperatureMeters.OrderBy(s => s.OfficeNumber),
                OfficesDescending => allTemperatureMeters.OrderByDescending(s => s.OfficeNumber),
                _ => allTemperatureMeters.OrderBy(s => s.Name),
            };
            return allTemperatureMeters;
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
