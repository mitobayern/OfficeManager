namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TenantsController : Controller
    {
        private const string OwnerAscending = "owner_asc";
        private const string OwnerDescending = "owner_desc";
        private const string CompanyNameDescending = "name_desc";
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;
        private readonly ITenantsService tenantsService;

        public TenantsController(ApplicationDbContext dbContext, IOfficesService officesService, ITenantsService tenantsService)
        {
            this.dbContext = dbContext;
            this.officesService = officesService;
            this.tenantsService = tenantsService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTenantViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.tenantsService.CreateTenantAsync(input);

            return this.Redirect("/Administration/Tenants/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allTenants = this.tenantsService.GetAllTenants().Where(x => x.HasContract == true);

            allTenants = this.OrderTenantsAsync(sortOrder, currentFilter, searchString, pageNumber, allTenants);
            int pageSize = GetPageSize(rowsPerPage, allTenants);

            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<TenantOutputViewModel>.CreateAsync(allTenants.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<ViewResult> AllNoContract(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allTenants = this.tenantsService.GetAllTenants().Where(x => x.HasContract == false);

            allTenants = this.OrderTenantsAsync(sortOrder, currentFilter, searchString, pageNumber, allTenants);
            int pageSize = GetPageSize(rowsPerPage, allTenants);

            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<TenantOutputViewModel>.CreateAsync(allTenants.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Details(TenantIdViewModel input)
        {
            if (!this.ValidateTenant(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var currentTenant = this.tenantsService.GetTenantById(input.Id);
            var tenantToEdit = this.tenantsService.EditTenant(currentTenant);

            return this.View(tenantToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Details(TenantToEditViewModel input)
        {
            var currentTenant = this.tenantsService.GetTenantById(input.Id);
            if (!this.ModelState.IsValid)
            {
                var tenantToEdit = this.tenantsService.EditTenant(currentTenant);
                input.Offices = tenantToEdit.Offices;
                return this.View(input);
            }

            await this.tenantsService.UpdateTenantAsync(input);
            if (!currentTenant.HasContract)
            {
                return this.Redirect("/Administration/Tenants/AllNoContract");
            }

            return this.Redirect("/Administration/Tenants/All");
        }

        public IActionResult AddOffices(TenantIdViewModel input)
        {
            if (!this.ValidateTenant(input.Id))
            {
                return this.Redirect("/Home/Error");
            }

            var availableOffices = this.officesService.GetAllAvailableOffices().ToList();

            return this.View(new TenantWithAllOfficesViewModel { Id = input.Id, AvailavleOffices = availableOffices });
        }

        [HttpPost]
        public async Task<IActionResult> AddOffices(AddRemoveOfficeViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("AddOffices", new TenantIdViewModel { Id = input.Id });
            }

            await this.officesService.AddOfficesToTenantAsync(input.Id, input.AreChecked);

            return this.Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
        }

        public IActionResult RemoveOffices(TenantIdViewModel input)
        {
            var currentTenantOffices = this.tenantsService.GetTenantOffices(input).ToList();

            return this.View(new TenantWithAllOfficesViewModel { Id = input.Id, CurrentOffices = currentTenantOffices });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOfficesAsync(AddRemoveOfficeViewModel input)
        {
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("RemoveOffices", new TenantIdViewModel { Id = input.Id });
            }

            await this.officesService.RemoveOfficesFromTenantAsync(input.Id, input.AreChecked);

            return this.Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TenantIdViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.tenantsService.DeleteTenantAsync(input.Id);

            return this.Redirect("/Administration/Tenants/All");
        }

        [HttpPost]
        public async Task<IActionResult> SignContract(TenantIdViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.tenantsService.SignContract(input.Id);

            return this.Redirect("/Administration/Tenants/All");
        }

        private static int GetPageSize(string rowsPerPage, IQueryable<TenantOutputViewModel> allTenants)
        {
            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allTenants.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            return pageSize;
        }

        private IQueryable<TenantOutputViewModel> OrderTenantsAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<TenantOutputViewModel> allTenants)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["CompanyNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? CompanyNameDescending : string.Empty;
            this.ViewData["OwnerSortParam"] = sortOrder == OwnerAscending ? OwnerDescending : OwnerAscending;

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
                allTenants = allTenants.Where(s => s.CompanyName.Contains(searchString)
                                       || s.CompanyOwner.Contains(searchString));
            }

            allTenants = sortOrder switch
            {
                CompanyNameDescending => allTenants.OrderByDescending(s => s.CompanyName),
                OwnerAscending => allTenants.OrderBy(s => s.CompanyOwner),
                OwnerDescending => allTenants.OrderByDescending(s => s.CompanyOwner),
                _ => allTenants.OrderBy(s => s.CompanyName),
            };

            return allTenants;
        }

        private bool ValidateTenant(int id)
        {
            if (this.dbContext.Tenants.Any(x => x.Id == id))
            {
                return true;
            }

            return false;
        }
    }
}
