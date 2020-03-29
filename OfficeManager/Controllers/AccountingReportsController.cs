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
            var allTenants = this.dbContext.Tenants.Select(x => x.CompanyName).ToList();

            var allPeriods = this.dbContext.ElectricityMeasurements.OrderByDescending(x => x.Id).Select(x => x.Period).ToList();
            List<string> outputPeriods = new List<string>();
            List<SelectListItem> periods = new List<SelectListItem>();
            List<SelectListItem> tenants = new List<SelectListItem>();


            foreach (var period in allPeriods)
            {
                if (!outputPeriods.Contains(period))
                {
                    outputPeriods.Add(period);
                    periods.Add(new SelectListItem
                    {
                        Text = period,
                        Value = period
                    });
                }
            }

            foreach (var tenant in allTenants)
            {
                tenants.Add(new SelectListItem
                {
                    Text = tenant,
                    Value = tenant
                });
            }

            return this.View(new TenantsAndPeriodsViewModel { Tenants = tenants, Periods = periods });
        }

        public IActionResult Generate(TenantsAndPeriodsViewModel input)
        {
            int number = 0;
            if (this.dbContext.AccountingReports.Count() == 0)
            {
                number = 1;
            }
            else
            {
                number = this.dbContext.AccountingReports.Count() + 1;
            }

            var landlord = this.landlordsService.GetLandlord();
            var tenant = this.tenantsService.GetTenantByName(input.Tenant);

            string eik = string.Empty;
            if (tenant.Bulstat.StartsWith("BG"))
            {
                eik = tenant.Bulstat.Substring(2);
            }
            else
            {
                eik = tenant.Bulstat;
            }

            string offices = string.Empty;
            if (tenant.Offices.Count > 1)
            {
                offices = "офиси ";
                List<string> currentTenantOffices = tenant.Offices.OrderBy(x => x.Name).Select(x => x.Name).ToList();

                for (int i = 0; i < currentTenantOffices.Count - 1; i++)
                {
                    if (currentTenantOffices[i] != currentTenantOffices[currentTenantOffices.Count - 2])
                    {
                        offices += currentTenantOffices[i] + ", ";
                    }
                    else
                    {
                        offices += currentTenantOffices[i] + " и ";
                    }
                }
                offices += currentTenantOffices[currentTenantOffices.Count - 1];
            }
            else
            {
                offices = "офис " + string.Join(", ", tenant.Offices.OrderBy(x => x.Name).Select(x => x.Name));
            }

            var pricesInformation = this.pricesInformationService.GetCurrentPrices();
            var electricityConsummation = this.measurementsService.GetTenantElectricityConsummationByPeriod(tenant, input.Period);
            var temperatureConsummation = this.measurementsService.GetTenantTemperatureConsummationByPeriod(tenant.CompanyName, input.Period);
            var amountForElectricity = (electricityConsummation.DayTimeConsummation + electricityConsummation.NightTimeConsummation)
                                    * (pricesInformation.AccessToDistributionGrid + pricesInformation.NetworkTaxesAndUtilities
                                    + pricesInformation.Excise + pricesInformation.ElectricityPerKWh);
            var amountForHeating = temperatureConsummation.HeatingConsummation * pricesInformation.HeatingPerKWh;
            var amountForCooling = temperatureConsummation.CoolingConsummation * pricesInformation.CoolingPerKWh;
            var totalAmount = (amountForElectricity + amountForHeating + amountForCooling) * 1.20M;

            var tenantInfo = new TenantAccountingReportViewModel
            {
                Id = tenant.Id,
                CompanyName = tenant.CompanyName,
                Address = tenant.Address,
                Bulstat = tenant.Bulstat,
                EIK = eik,
                CompanyOwner = tenant.CompanyOwner,
                StartOfContract = tenant.StartOfContract,
                Offices = offices,
            };

            AccountingReportViewModel accountingReport = new AccountingReportViewModel
            {
                Number = number,
                Landlord = landlord,
                Tenant = tenantInfo,
                CreatedOn = DateTime.UtcNow,
                Period = input.Period,
                PricesInformation = pricesInformation,
                DayTimeElectricityConsummation = electricityConsummation.DayTimeConsummation,
                NightTimeElectricityConsummation = electricityConsummation.NightTimeConsummation,
                AmountForElectricity = amountForElectricity,
                HeatingConsummation = temperatureConsummation.HeatingConsummation,
                AmountForHeating = amountForHeating,
                CoolingConsummation = temperatureConsummation.CoolingConsummation,
                AmountForCooling = amountForCooling,
                PricesInformationId = pricesInformation.Id,
                TenantId = tenant.Id,
                TotalAmount = totalAmount,
            };

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