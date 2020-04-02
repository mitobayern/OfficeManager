namespace OfficeManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.AccountingReports;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountingReportsController : Controller
    {
        private readonly IAccontingReportsService accontingReportsService;

        public AccountingReportsController(IAccontingReportsService accontingReportsService)
        {
            this.accontingReportsService = accontingReportsService;
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
            return this.RedirectToAction("Generate", input);
        }

        public IActionResult Generate(TenantsAndPeriodsViewModel input)
        {
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