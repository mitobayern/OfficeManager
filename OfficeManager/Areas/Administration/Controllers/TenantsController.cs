namespace OfficeManager.Areas.Administration.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using OfficeManager.Services;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.Tenants;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class TenantsController : Controller
    {
        private readonly IOfficesService officesService;
        private readonly ITenantsService tenantsService;

        public TenantsController(IOfficesService officesService, ITenantsService tenantsService)
        {
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
            tenantsService.CreateTenant(input);

            return Redirect("/Administration/Tenants/All");
        }

        public IActionResult All()
        {
            var allTenants = tenantsService.GetAllTenants().ToList();

            return View(new AllTenantsViewModel { Tenants = allTenants });
        }

        public IActionResult Details(TenantIdViewModel input)
        {
            var currentTenant = tenantsService.GetTenantById(input.Id);
            var tenantToEdit = tenantsService.EditTenant(currentTenant);

            return View(tenantToEdit);
        }

        [HttpPost]
        public IActionResult Details(TenantToEditViewModel input)
        {
            if (!ModelState.IsValid)
            {
                var currentTenant = tenantsService.GetTenantById(input.Id);
                var tenantToEdit = tenantsService.EditTenant(currentTenant);
                input.Offices = tenantToEdit.Offices;
                return View(input);
            }
            tenantsService.UpdateTenant(input);

            return Redirect("/Administration/Tenants/All");
        }

        public IActionResult AddOffices(TenantIdViewModel input)
        {
            var availableOffices = officesService.GetAllAvailableOffices().ToList();

            return View(new TenantWithAllOfficesViewModel { Id = input.Id, AvailavleOffices = availableOffices });
        }

        [HttpPost]
        public IActionResult AddOffices(AddRemoveOfficeViewModel input)
        {
            officesService.AddOfficesToTenant(input.Id, input.AreChecked);

            return Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
        }

        public IActionResult RemoveOffices(TenantIdViewModel input)
        {
            var currentTenantOffices = tenantsService.GetTenantOffices(input).ToList();

            return View(new TenantWithAllOfficesViewModel { Id = input.Id, CurrentOffices = currentTenantOffices });
        }

        [HttpPost]
        public IActionResult RemoveOffices(AddRemoveOfficeViewModel input)
        {
            officesService.RemoveOfficeFromTenant(input.Id, input.AreChecked);

            return Redirect("/Administration/Tenants/Details?id=" + input.Id.ToString());
        }
    }
}
