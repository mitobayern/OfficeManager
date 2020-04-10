namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;
    using OfficeManager.Data;
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TenantsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IOfficesService officesService;
        private readonly ITenantsService tenantsService;
        private const string companyNameAscending = "name_asc";
        private const string companyNameDescending = "name_desc";
        private const string ownerAscending = "owner_asc";
        private const string ownerDescending = "owner_desc";
        

        public TenantsController(ApplicationDbContext dbContext, IOfficesService officesService, ITenantsService tenantsService)
        {
            this.dbContext = dbContext;
            this.officesService = officesService;
            this.tenantsService = tenantsService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTenantViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            this.tenantsService.CreateTenant(input);

            return Redirect("/Administration/Tenants/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allTenants = tenantsService.GetAllTenants();

            allTenants = OrderTenantsAsync(sortOrder, currentFilter, searchString, pageNumber, allTenants);

            int pageSize;

            if (String.IsNullOrEmpty(rowsPerPage))
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

            ViewData["RowsPerPage"] = pageSize;
            
            return View(await PaginatedList<TenantOutputViewModel>.CreateAsync(allTenants.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private IQueryable<TenantOutputViewModel> OrderTenantsAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<TenantOutputViewModel> allTenants)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["order"] = "Company: Z to A";
            ViewData["CompanyNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? companyNameDescending : "";
            ViewData["OwnerSortParam"] = sortOrder == ownerAscending ? ownerDescending : ownerAscending;
           

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
                allTenants = allTenants.Where(s => s.CompanyName.Contains(searchString)
                                       || s.CompanyOwner.Contains(searchString));
            }

            switch (sortOrder)
            {
                case companyNameDescending:
                    allTenants = allTenants.OrderByDescending(s => s.CompanyName);
                    ViewData["order"] = companyNameDescending;
                    break;
                case ownerAscending:
                    allTenants = allTenants.OrderBy(s => s.CompanyOwner);
                    ViewData["order"] = ownerAscending;
                    break;
                case ownerDescending:
                    allTenants = allTenants.OrderByDescending(s => s.CompanyOwner);
                    ViewData["order"] = ownerDescending;
                    break;
                default:
                    allTenants = allTenants.OrderBy(s => s.CompanyName);
                    ViewData["order"] = companyNameAscending;
                    break;
            }
            return allTenants;
        }







        public IActionResult Details(TenantIdViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }

            var currentTenant = tenantsService.GetTenantById(input.Id);
            var tenantToEdit = tenantsService.EditTenant(currentTenant);

            return View(tenantToEdit);
        }

        [HttpPost]
        public IActionResult Details(TenantToEditViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }

            if (!ModelState.IsValid)
            {
                var currentTenant = tenantsService.GetTenantById(input.Id);
                var tenantToEdit = tenantsService.EditTenant(currentTenant);
                input.Offices = tenantToEdit.Offices;
                return View(input);
            }

            this.tenantsService.UpdateTenant(input);

            return Redirect("/Administration/Tenants/All");
        }

        public IActionResult AddOffices(TenantIdViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }

            var availableOffices = officesService.GetAllAvailableOffices().ToList();

            return View(new TenantWithAllOfficesViewModel { Id = input.Id, AvailavleOffices = availableOffices });
        }

        [HttpPost]
        public IActionResult AddOffices(AddRemoveOfficeViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("AddOffices", new TenantIdViewModel { Id = input.Id });
            }

            this.officesService.AddOfficesToTenant(input.Id, input.AreChecked);

            return Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
        }

        public IActionResult RemoveOffices(TenantIdViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }

            var currentTenantOffices = tenantsService.GetTenantOffices(input).ToList();

            return View(new TenantWithAllOfficesViewModel { Id = input.Id, CurrentOffices = currentTenantOffices });
        }

        [HttpPost]
        public IActionResult RemoveOffices(AddRemoveOfficeViewModel input)
        {
            if (!ValidateTenant(input.Id))
            {
                return this.Redirect("/Administration/Tenants/All");
            }
            if (input.AreChecked == null)
            {
                return this.RedirectToAction("RemoveOffices", new TenantIdViewModel { Id = input.Id });
            }

            this.officesService.RemoveOfficesFromTenant(input.Id, input.AreChecked);

            return Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
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
