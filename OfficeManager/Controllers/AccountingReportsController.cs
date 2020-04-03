namespace OfficeManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.AccountingReports;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountingReportsController : Controller
    {
        private readonly IAccontingReportsService accontingReportsService;
        private readonly ApplicationDbContext dbContext;

        public AccountingReportsController(IAccontingReportsService accontingReportsService, ApplicationDbContext dbContext)
        {
            this.accontingReportsService = accontingReportsService;
            this.dbContext = dbContext;
        }

        public IActionResult Create()
        {
            var tenantsAndPeriods = new TenantsAndPeriodsViewModel
            {
                Tenants = this.accontingReportsService.GetAllTenants(),
                Periods = this.accontingReportsService.GetAllPeriods(),
                AccountingReports = this.accontingReportsService.GetAllAccountingReports().ToList(),
            };

            return this.View(tenantsAndPeriods);
        }

        [HttpPost]
        public IActionResult Create(TenantsAndPeriodsViewModel input)
        {
            if (!ModelState.IsValid)
            {
                var tenantsAndPeriods = new TenantsAndPeriodsViewModel
                {
                    Tenant = input.Tenant,
                    Period = input.Period,
                    Tenants = this.accontingReportsService.GetAllTenants(),
                    Periods = this.accontingReportsService.GetAllPeriods(),
                    AccountingReports = this.accontingReportsService.GetAllAccountingReports().ToList(),
                };

                return this.View(tenantsAndPeriods);
            }
            var result = new AccountingReportInputViewModel
            {
                Tenant = input.Tenant,
                Period = input.Period,
            };

            return this.RedirectToAction("Generate", result);
        }

        public IActionResult Generate(AccountingReportInputViewModel input)
        {
            var AccountingReports = this.accontingReportsService.GetAllAccountingReports().ToList();
            if (AccountingReports != null)
            {
                var MatchingAccountingReports = AccountingReports.Any(x => x.CompanyName == input.Tenant && x.Period == input.Period);
                var ExistingTenant = this.dbContext.Tenants.Any(x=>x.CompanyName == input.Tenant);
                var ExistingPeriod = this.dbContext.ElectricityMeasurements.Any(x => x.Period == input.Period);

                if (MatchingAccountingReports || !ExistingTenant || !ExistingPeriod)
                {
                    return this.Redirect("/AccountingReports/Create");
                }
            }

            var accountingReport = this.accontingReportsService.GetAccountingReportViewModel(input.Tenant, input.Period);
            var accountingReportJson = JsonConvert.SerializeObject(accountingReport, Formatting.Indented);

            return this.View(new AccountingReportWithJson { Json = accountingReportJson, AccountingReport = accountingReport });
        }

        [HttpPost]
        public IActionResult Generate(AccountingReportWithJson input)
        {
            var accountingReport = JsonConvert.DeserializeObject<AccountingReportViewModel>(input.Json);
            this.accontingReportsService.GenerateAccountingReport(accountingReport);

            return this.Redirect("/AccountingReports/All");
        }

        public IActionResult All()
        {
            var allAccountingReports = this.accontingReportsService.GetAllAccountingReports().ToList();

            return this.View(new AllAccountingReportsViewModel { AccountingReports = allAccountingReports });
        }

        public IActionResult Details(AccountingReportIdViewModel input)
        {
            var accountingReport = this.accontingReportsService.GetAccountingReportById(input.Id);

            return this.View(accountingReport);
        }
    }
}