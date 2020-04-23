namespace OfficeManager.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using OfficeManager.Areas.Administration.ViewModels.Charts;
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
            var accountingReports = this.accountingReportsService.GetAllAccountingReports().Where(x => x.CompanyName == companyName).OrderBy(x => x.CreatedOn).Take(12).
                Select(x => new ChartOutputViewModel
                {
                    Period = x.Period,
                    AmountForElectricity = x.AmountForElectricity,
                    AmountForHeating = x.AmountForHeating,
                    AmountForCooling = x.AmountForCooling,
                }).ToList();

            var periods = new List<string>();
            var amountsForElectricity = new List<decimal>();
            var amountsForHeating = new List<decimal>();
            var amountsForCooling = new List<decimal>();

            foreach (var accountingReport in accountingReports)
            {
                periods.Add(accountingReport.Period);
                amountsForElectricity.Add(accountingReport.AmountForElectricity);
                amountsForHeating.Add(accountingReport.AmountForHeating);
                amountsForCooling.Add(accountingReport.AmountForCooling);
            }

            var result = new ChartsJsonOutputViewModel
            {
                Periods = JsonConvert.SerializeObject(periods, Formatting.Indented),
                AmountsForElectricity = JsonConvert.SerializeObject(amountsForElectricity, Formatting.Indented),
                AmountsForHeating = JsonConvert.SerializeObject(amountsForHeating, Formatting.Indented),
                AmountsForCooling = JsonConvert.SerializeObject(amountsForCooling, Formatting.Indented),
            };

            this.ViewData["TenantId"] = id;

            this.ViewData["Title"] = companyName;

            return this.View(result);
        }
    }
}
