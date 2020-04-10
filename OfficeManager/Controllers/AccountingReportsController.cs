namespace OfficeManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.AccountingReports;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountingReportsController : Controller
    {
        private readonly IAccontingReportsService accontingReportsService;
        private readonly ApplicationDbContext dbContext;
        private const string numberAscending = "number_asc";
        private const string numberDescending = "number_desc";
        private const string dateAscending = "date_asc";
        private const string dateDescending = "date_desc";
        private const string tenantsAscending = "tenant_asc";
        private const string tenantsDescending = "tenant_desc";
        private const string periodAscending = "period_asc";
        private const string periodDescending = "period_desc";
        private const string totalAmountAscending = "amount_asc";
        private const string totalAmountDescending = "amount_desc";

        public AccountingReportsController(ApplicationDbContext dbContext, IAccontingReportsService accontingReportsService)
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
            if (!ValidateTenantAndPeriod(input.Tenant, input.Period))
            {
                return this.Redirect("/AccountingReports/Create");
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

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            var allAccountingReports = this.accontingReportsService.GetAllAccountingReports();

            allAccountingReports = OrderAccountingReportsAsync(sortOrder, currentFilter, searchString, pageNumber, allAccountingReports);


            int pageSize;

            if (String.IsNullOrEmpty(rowsPerPage))
            {
                pageSize = 5;
            }
            else if (rowsPerPage == "All")
            {
                pageSize = allAccountingReports.Count();
            }
            else
            {
                pageSize = int.Parse(rowsPerPage);
            }

            ViewData["RowsPerPage"] = pageSize;

            return View(await PaginatedList<AccountingReportListViewModel>.CreateAsync(allAccountingReports.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private IQueryable<AccountingReportListViewModel> OrderAccountingReportsAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<AccountingReportListViewModel> allAccountingReports)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["order"] = "Company: Z to A";
            ViewData["NumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? numberDescending : "";
            ViewData["DateSortParam"] = sortOrder == dateAscending ? dateDescending : dateAscending;
            ViewData["TenantSortParam"] = sortOrder == tenantsAscending ? tenantsDescending : tenantsAscending;
            ViewData["PeriodSortParam"] = sortOrder == periodAscending ? periodDescending : periodAscending;
            ViewData["TotalAmountSortParam"] = sortOrder == totalAmountAscending ? totalAmountDescending : totalAmountAscending;

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
                allAccountingReports = allAccountingReports.Where(s => s.CompanyName.Contains(searchString)
                                       || s.Number.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case numberDescending:
                    allAccountingReports = allAccountingReports.OrderByDescending(s => s.Number);
                    break;
                case dateAscending:
                    allAccountingReports = allAccountingReports.OrderBy(s => s.CreatedOn);
                    break;
                case dateDescending:
                    allAccountingReports = allAccountingReports.OrderByDescending(s => s.CreatedOn);
                    break;
                case tenantsAscending:
                    allAccountingReports = allAccountingReports.OrderBy(s => s.CompanyName);
                    break;
                case tenantsDescending:
                    allAccountingReports = allAccountingReports.OrderByDescending(s => s.CompanyName);
                    break;
                case periodAscending:
                    allAccountingReports = allAccountingReports.OrderBy(s => s.Period);
                    break;
                case periodDescending:
                    allAccountingReports = allAccountingReports.OrderByDescending(s => s.Period);
                    break;
                case totalAmountAscending:
                    allAccountingReports = allAccountingReports.OrderBy(s => s.TotalAmount);
                    break;
                case totalAmountDescending:
                    allAccountingReports = allAccountingReports.OrderByDescending(s => s.TotalAmount);
                    break;
                default:
                    allAccountingReports = allAccountingReports.OrderBy(s => s.Number);
                    break;
            }
            return allAccountingReports;
        }








        public IActionResult Details(AccountingReportIdViewModel input)
        {
            var accountingReport = this.accontingReportsService.GetAccountingReportById(input.Id);

            return this.View(accountingReport);
        }

        private bool ValidateTenantAndPeriod(string tenant, string period)
        {
            var AccountingReports = this.accontingReportsService.GetAllAccountingReports().ToList();
            if (AccountingReports != null)
            {
                var MatchingAccountingReports = AccountingReports.Any(x => x.CompanyName == tenant && x.Period == period);
                var ExistingTenant = this.dbContext.Tenants.Any(x => x.CompanyName == tenant);
                var ExistingPeriod = this.dbContext.ElectricityMeasurements.Any(x => x.Period == period);

                if (MatchingAccountingReports || !ExistingTenant || !ExistingPeriod)
                {
                    return false;
                }
            }

            return true;
        }
    }
}