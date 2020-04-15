namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OfficeManager.Data;
    using OfficeManager.Services;

    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAccountingReportsService accountingReportsService;
        private readonly ITenantsService tenantsService;

        public ChartsController(ApplicationDbContext dbContext, IAccountingReportsService accountingReportsService, ITenantsService tenantsService)
        {
            this.dbContext = dbContext;
            this.accountingReportsService = accountingReportsService;
            this.tenantsService = tenantsService;
        }

        public IActionResult Index(int id)
        {
            var companyName = this.tenantsService.GetTenantById(id).CompanyName;
            var accountingReports = this.accountingReportsService.GetAllAccountingReports().Where(x => x.CompanyName == companyName).OrderBy(x => x.CreatedOn).Take(12).ToList();

            return this.View(accountingReports);
        }
    }
}
