namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private const string AreaAscending = "area_asc";
        private const string AreaDescending = "area_desc";
        private const string RentAscending = "rent_asc";
        private const string RentDescending = "rent_desc";
        private const string OfficesDescending = "office_desc";
        private const string TenantsAscending = "tenant_asc";
        private const string TenantsDescending = "tenant_desc";
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;
        private readonly ITemperatureMetersService temperatureMetersService;

        public OfficesController(
            ApplicationDbContext dbContext,
            IOfficesService officesService,
            ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.officesService = officesService;
            this.temperatureMetersService = temperatureMetersService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOfficeViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.officesService.CreateOfficeAsync(input.Name, input.Area, input.RentPerSqMeter);

            return this.Redirect("/Administration/Offices/All");
        }

        public IActionResult Edit(OfficeIdViewModel input)
        {
            if (!this.ValidateOffice(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var officeToEdit = this.officesService.EditOffice(input.Id);

            return this.View(officeToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditOfficeViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var currentOffice = this.officesService.EditOffice(input.Id);

                return this.View(currentOffice);
            }

            await this.officesService.UpdateOfficeAsync(input.Id, input.Name, input.Area, input.RentPerSqMeter);

            return this.Redirect("/Administration/Offices/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allOffices = this.officesService.GetAllOffices();

            allOffices = this.OrderOffices(sortOrder, currentFilter, searchString, pageNumber, allOffices);
            int pageSize = GetPageSize(rowsPerPage, allOffices);

            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<OfficeOutputViewModel>.CreateAsync(allOffices.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult AddTemperatureMeters(OfficeIdViewModel input)
        {
            if (!this.ValidateOffice(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var allAvailableTemperatureMeters = this.temperatureMetersService
                .GetAllTemperatureMeters()
                .Where(x => x.OfficeNumber == null)
                .ToList();

            return this.View(new OfficeWithAllTemperatureMetersViewModel { Id = input.Id, AvailavleTemperatureMeters = allAvailableTemperatureMeters });
        }

        [HttpPost]
        public async Task<IActionResult> AddTemperatureMetersAsync(AddRemoveTemperatureMetersViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("AddTemperatureMeters", new OfficeIdViewModel { Id = input.Id });
            }

            await this.officesService.AddTemperatureMetersToOfficeAsync(input.Id, input.AreChecked);

            return this.Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult RemoveTemperatureMeters(OfficeIdViewModel input)
        {
            var currentOfficeTemperatreMeters = this.officesService.GetOfficeTemperatureMeters(input.Id).ToList();

            return this.View(new OfficeWithCurrentTemperatureMetersViewModel { Id = input.Id, CurrentTemperatureMeters = currentOfficeTemperatreMeters });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTemperatureMetersAsync(AddRemoveTemperatureMetersViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("RemoveTemperatureMeters", new OfficeIdViewModel { Id = input.Id });
            }

            await this.officesService.RemoveTemperatureMetersFromOfficeAsync(input.Id, input.AreChecked);

            return this.Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public IActionResult AddElectricityMeter(OfficeIdViewModel input)
        {
            if (!this.ValidateOffice(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var office = this.dbContext.Offices.FirstOrDefault(x => x.Id == input.Id);

            var elMeters = this.dbContext.ElectricityMeters
                .Where(x => x.Office == null)
                .Select(x => new ElectricityMeterOutputViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    OfficeNumber = x.Office.Name,
                    PowerSupply = x.PowerSupply,
                }).ToList();

            return this.View(new OfficeWithAllElectricityMetersViewModel { Id = input.Id, AvailavleElectricityMeters = elMeters });
        }

        [HttpPost]
        public async Task<IActionResult> AddElectricityMeterAsync(AddRemoveElectricityMeterViewModel input)
        {
            if (input.IsChecked == null)
            {
                return this.RedirectToAction("AddElectricityMeter", new OfficeIdViewModel { Id = input.Id });
            }

            await this.officesService.AddElectricityMeterToOfficeAsync(input.Id, input.IsChecked);

            return this.Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        public async Task<IActionResult> RemoveElectricityMeterAsync(OfficeIdViewModel input)
        {
            if (!this.ValidateOffice(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            await this.officesService.RemoveElectricityMeterFromOfficeAsync(input.Id);

            return this.Redirect("/Administration/Offices/Edit?id=" + input.Id.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OfficeIdViewModel input)
        {
            await this.officesService.DeleteOfficeAsync(input.Id);

            return this.Redirect("/Administration/Offices/All");
        }

        private static int GetPageSize(string rowsPerPage, IQueryable<OfficeOutputViewModel> allOffices)
        {
            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allOffices.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            return pageSize;
        }

        private IQueryable<OfficeOutputViewModel> OrderOffices(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<OfficeOutputViewModel> allOffices)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["OfficeNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? OfficesDescending : string.Empty;
            this.ViewData["TenantSortParam"] = sortOrder == TenantsAscending ? TenantsDescending : TenantsAscending;
            this.ViewData["AreaSortParam"] = sortOrder == AreaAscending ? AreaDescending : AreaAscending;
            this.ViewData["RentSortParam"] = sortOrder == RentAscending ? RentDescending : RentAscending;

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
                allOffices = allOffices.Where(s => s.Name.Contains(searchString)
                                       || s.TenantName.Contains(searchString));
            }

            allOffices = sortOrder switch
            {
                OfficesDescending => allOffices.OrderByDescending(s => s.Name),
                TenantsAscending => allOffices.OrderBy(s => s.TenantName),
                TenantsDescending => allOffices.OrderByDescending(s => s.TenantName),
                AreaAscending => allOffices.OrderBy(s => s.Area),
                AreaDescending => allOffices.OrderByDescending(s => s.Area),
                RentAscending => allOffices.OrderBy(s => s.RentPerSqMeter),
                RentDescending => allOffices.OrderByDescending(s => s.RentPerSqMeter),
                _ => allOffices.OrderBy(s => s.Name),
            };
            return allOffices;
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
