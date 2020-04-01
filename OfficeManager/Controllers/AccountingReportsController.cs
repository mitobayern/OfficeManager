using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OfficeManager.Data;
using OfficeManager.Services;
using OfficeManager.ViewModels.AccountingReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Controllers
{
    public class AccountingReportsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITenantsService tenantsService;
        private readonly ILandlordsService landlordsService;
        private readonly IPricesInformationService pricesInformationService;
        private readonly IMeasurementsService measurementsService;
        private readonly IAccontingReportsService accontingReportsService;

        public AccountingReportsController(ApplicationDbContext dbContext,
                                           ITenantsService tenantsService,
                                           ILandlordsService landlordsService,
                                           IPricesInformationService pricesInformationService,
                                           IMeasurementsService measurementsService,
                                           IAccontingReportsService accontingReportsService)
        {
            this.dbContext = dbContext;
            this.tenantsService = tenantsService;
            this.landlordsService = landlordsService;
            this.pricesInformationService = pricesInformationService;
            this.measurementsService = measurementsService;
            this.accontingReportsService = accontingReportsService;
        }

        public IActionResult Create()
        {
            var tenantsAndPeriods = new TenantsAndPeriodsViewModel
            {
                Tenants = this.accontingReportsService.GetAllTenants(),
                Periods = this.accontingReportsService.GetAllPeriods(),
            };

            return this.View(tenantsAndPeriods);
        }

        public IActionResult Generate(TenantsAndPeriodsViewModel input)
        {
            //int number = 0;
            //if (this.dbContext.AccountingReports.Count() == 0)
            //{
            //    number = 1;
            //}
            //else
            //{
            //    number = this.dbContext.AccountingReports.Count() + 1;
            //}

            //var landlord = this.landlordsService.GetLandlord();

            //var tenant = this.tenantsService.GetTenantByCompanyName(input.Tenant);

            //var pricesInformation = this.pricesInformationService.GetCurrentPrices();

            //var electricityConsummation = this.accontingReportsService.GetTenantElectricityConsummationByPeriod(tenant.CompanyName, input.Period);
            //var amountForElectricity = this.accontingReportsService.AmountForElectricity(electricityConsummation);
            //var temperatureConsummation = this.accontingReportsService.GetTenantTemperatureConsummationByPeriod(tenant.CompanyName, input.Period);

            //var amountForHeating = temperatureConsummation.HeatingConsummation * pricesInformation.HeatingPerKWh;
            //var amountForCooling = temperatureConsummation.CoolingConsummation * pricesInformation.CoolingPerKWh;
            //var totalAmount = (amountForElectricity + amountForHeating + amountForCooling) * 1.20M;

            //var tenantInfo = new TenantAccountingReportViewModel
            //{
            //    Id = tenant.Id,
            //    CompanyName = tenant.CompanyName,
            //    Address = tenant.Address,
            //    Bulstat = tenant.Bulstat,
            //    EIK = this.tenantsService.GetTenantEIK(input.Tenant),
            //    CompanyOwner = tenant.CompanyOwner,
            //    StartOfContract = tenant.StartOfContract,
            //    Offices = this.tenantsService.GetTenantOfficesAsText(input.Tenant),
            //};

            //AccountingReportViewModel accountingReport = new AccountingReportViewModel
            //{
            //    Number = number,
            //    Landlord = landlord,
            //    Tenant = tenantInfo,
            //    CreatedOn = DateTime.UtcNow,
            //    Period = input.Period,
            //    PricesInformation = pricesInformation,
            //    DayTimeElectricityConsummation = electricityConsummation.DayTimeConsummation,
            //    NightTimeElectricityConsummation = electricityConsummation.NightTimeConsummation,
            //    AmountForElectricity = this.accontingReportsService.AmountForElectricity(electricityConsummation),
            //    HeatingConsummation = temperatureConsummation.HeatingConsummation,
            //    AmountForHeating = amountForHeating,
            //    CoolingConsummation = temperatureConsummation.CoolingConsummation,
            //    AmountForCooling = amountForCooling,
            //    PricesInformationId = pricesInformation.Id,
            //    TenantId = tenant.Id,
            //    TotalAmount = totalAmount,
            //};
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