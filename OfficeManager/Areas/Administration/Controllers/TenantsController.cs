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

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TenantsController : Controller
    {
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

        public IActionResult All(string sortOrder)
        {
            ViewData["order"] = "Company: Z to A";
            ViewData["CompanyNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "company_desc" : "";
            ViewData["OwnerSortParam"] = sortOrder == "owner_asc" ? "owner_desc" : "owner_asc";


            var allTenants = tenantsService.GetAllTenants().ToList();

            switch (sortOrder)
            {
                case "company_desc":
                    allTenants = allTenants.OrderByDescending(s => s.CompanyName).ToList();
                    ViewData["order"] = "company_desc";
                    break;
                case "owner_asc":
                    allTenants = allTenants.OrderBy(s => s.CompanyOwner).ToList();
                    ViewData["order"] = "owner_asc";
                    break;
                case "owner_desc":
                    allTenants = allTenants.OrderByDescending(s => s.CompanyOwner).ToList();
                    ViewData["order"] = "owner_desc";
                    break;
                default:
                    allTenants = allTenants.OrderBy(s => s.CompanyName).ToList();
                    ViewData["order"] = "company_asc";
                    break;
            }
            return View(new AllTenantsViewModel { Tenants = allTenants });
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
