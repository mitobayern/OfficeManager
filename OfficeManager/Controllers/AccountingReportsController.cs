namespace OfficeManager.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.AccountingReports;

    public class AccountingReportsController : Controller
    {
        private const string NumberAscending = "number_asc";
        private const string DateAscending = "date_asc";
        private const string DateDescending = "date_desc";
        private const string TenantsAscending = "tenant_asc";
        private const string TenantsDescending = "tenant_desc";
        private const string PeriodAscending = "period_asc";
        private const string PeriodDescending = "period_desc";
        private const string TotalAmountAscending = "amount_asc";
        private const string TotalAmountDescending = "amount_desc";
        private readonly IAccountingReportsService accountingReportsService;
        private readonly IViewRenderService viewRenderService;
        private readonly IHtmlToPdfConverter htmlToPdfConverter;
        private readonly IHostingEnvironment environment;
        private readonly ApplicationDbContext dbContext;

        public AccountingReportsController(
            ApplicationDbContext dbContext,
            IAccountingReportsService accontingReportsService,
            IViewRenderService viewRenderService,
            IHtmlToPdfConverter htmlToPdfConverter,
            IHostingEnvironment environment)
        {
            this.accountingReportsService = accontingReportsService;
            this.viewRenderService = viewRenderService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
            this.dbContext = dbContext;
        }

        public IActionResult Create()
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var tenantsAndPeriods = new TenantsAndPeriodsViewModel
            {
                Tenants = this.accountingReportsService.GetAllTenantsSelectList(),
                Periods = this.accountingReportsService.GetAllPeriodsSelectList(),
                AccountingReports = this.accountingReportsService.GetAllAccountingReports().ToList(),
            };

            return this.View(tenantsAndPeriods);
        }

        [HttpPost]
        public IActionResult Create(TenantsAndPeriodsViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var tenantsAndPeriods = new TenantsAndPeriodsViewModel
                {
                    Tenant = input.Tenant,
                    Period = input.Period,
                    Tenants = this.accountingReportsService.GetAllTenantsSelectList(),
                    Periods = this.accountingReportsService.GetAllPeriodsSelectList(),
                    AccountingReports = this.accountingReportsService.GetAllAccountingReports().ToList(),
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
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            if (!this.ValidateTenantAndPeriod(input.Tenant, input.Period))
            {
                return this.Redirect("/AccountingReports/Create");
            }

            var accountingReport = this.accountingReportsService.GetAccountingReportViewModel(input.Tenant, input.Period);
            var accountingReportJson = JsonConvert.SerializeObject(accountingReport, Formatting.Indented);

            return this.View(new AccountingReportWithJson { Json = accountingReportJson, AccountingReport = accountingReport });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateAsync(AccountingReportWithJson input)
        {
            var accountingReport = JsonConvert.DeserializeObject<AccountingReportViewModel>(input.Json);
            await this.accountingReportsService.GenerateAccountingReportAsync(accountingReport);

            return this.Redirect("/AccountingReports/All");
        }

        public async Task<ViewResult> All(string sortOrder, string currentFilter, string searchString, int? pageNumber, string rowsPerPage)
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var allAccountingReports = this.accountingReportsService.GetAllAccountingReports();

            allAccountingReports = this.OrderAccountingReportsAsync(sortOrder, currentFilter, searchString, pageNumber, allAccountingReports);
            int pageSize = this.GetPageSize(rowsPerPage, allAccountingReports);

            this.ViewData["TenantsCount"] = this.accountingReportsService.AlTenants().Count() / 2;
            this.ViewData["AllTenants"] = this.accountingReportsService.AlTenants();
            this.ViewData["AllPeriods"] = this.accountingReportsService.AllPeriods();
            this.ViewData["RowsPerPage"] = pageSize;

            return this.View(await PaginatedList<AccountingReportListViewModel>.CreateAsync(allAccountingReports.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Details(AccountingReportIdViewModel input)
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var accountingReport = this.accountingReportsService.GetAccountingReportById(input.Id);

            return this.View(accountingReport);
        }

        public async Task<IActionResult> GetPdf(int id)
        {
            if (!this.ValidateUser(this.User.Identity.Name))
            {
                return this.View("~/Views/Shared/Locked.cshtml");
            }

            var accountingReport = this.accountingReportsService.GetAccountingReportById(id);

            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/AccountingReports/Details.cshtml", accountingReport);
            var fileContents = this.htmlToPdfConverter.Convert(this.environment.ContentRootPath, htmlData);
            return this.File(fileContents, "application/pdf");
        }

        private IQueryable<AccountingReportListViewModel> OrderAccountingReportsAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, IQueryable<AccountingReportListViewModel> allAccountingReports)
        {
            this.ViewData["CurrentSort"] = sortOrder;
            this.ViewData["NumberSortParm"] = string.IsNullOrEmpty(sortOrder) ? NumberAscending : string.Empty;
            this.ViewData["DateSortParam"] = sortOrder == DateAscending ? DateDescending : DateAscending;
            this.ViewData["TenantSortParam"] = sortOrder == TenantsAscending ? TenantsDescending : TenantsAscending;
            this.ViewData["PeriodSortParam"] = sortOrder == PeriodAscending ? PeriodDescending : PeriodAscending;
            this.ViewData["TotalAmountSortParam"] = sortOrder == TotalAmountAscending ? TotalAmountDescending : TotalAmountAscending;

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
                allAccountingReports = allAccountingReports.Where(s => s.CompanyName.Contains(searchString)
                                       || s.Period.Contains(searchString));
            }

            allAccountingReports = sortOrder switch
            {
                NumberAscending => allAccountingReports.OrderBy(s => s.Number),
                DateAscending => allAccountingReports.OrderBy(s => s.CreatedOn),
                DateDescending => allAccountingReports.OrderByDescending(s => s.CreatedOn),
                TenantsAscending => allAccountingReports.OrderBy(s => s.CompanyName),
                TenantsDescending => allAccountingReports.OrderByDescending(s => s.CompanyName),
                PeriodAscending => allAccountingReports.OrderBy(s => s.Period),
                PeriodDescending => allAccountingReports.OrderByDescending(s => s.Period),
                TotalAmountAscending => allAccountingReports.OrderBy(s => s.TotalAmount),
                TotalAmountDescending => allAccountingReports.OrderByDescending(s => s.TotalAmount),
                _ => allAccountingReports.OrderByDescending(s => s.Number),
            };
            return allAccountingReports;
        }

        private int GetPageSize(string rowsPerPage, IQueryable<AccountingReportListViewModel> allAccountingReports)
        {
            int pageSize;

            if (string.IsNullOrEmpty(rowsPerPage))
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

            return pageSize;
        }

        private bool ValidateTenantAndPeriod(string tenant, string period)
        {
            var accountingReports = this.accountingReportsService.GetAllAccountingReports().ToList();
            if (accountingReports != null)
            {
                var matchingAccountingReports = accountingReports.Any(x => x.CompanyName == tenant && x.Period == period);
                var existingTenant = this.dbContext.Tenants.Any(x => x.CompanyName == tenant);
                var existingPeriod = this.dbContext.ElectricityMeasurements.Any(x => x.Period == period);

                if (matchingAccountingReports || !existingTenant || !existingPeriod)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateUser(string userName)
        {
            if (this.dbContext.Users.First(d => d.UserName == userName).IsEnabled == null)
            {
                return false;
            }

            return true;
        }
    }
}