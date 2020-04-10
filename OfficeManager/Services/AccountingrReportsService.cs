using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
using OfficeManager.Data;
using OfficeManager.Models;
using OfficeManager.ViewModels.AccountingReports;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public class AccountingReportsService : IAccontingReportsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITenantsService tenantsService;
        private readonly ILandlordsService landlordsService;
        private readonly IPricesInformationService pricesInformationService;

        public AccountingReportsService(ApplicationDbContext dbContext,
                                        ITenantsService tenantsService,
                                        ILandlordsService landlordsService,
                                        IPricesInformationService pricesInformationService)
        {
            this.dbContext = dbContext;
            this.tenantsService = tenantsService;
            this.landlordsService = landlordsService;
            this.pricesInformationService = pricesInformationService;
        }
        
        public List<SelectListItem> GetAllTenants()
        {
            var allTenants = this.dbContext.Tenants.Select(x => x.CompanyName).ToList();

            List<SelectListItem> tenantsViewModel = new List<SelectListItem>();

            foreach (var tenant in allTenants)
            {
                tenantsViewModel.Add(new SelectListItem
                {
                    Text = tenant,
                    Value = tenant
                });
            }

            return tenantsViewModel;
        }

        public List<SelectListItem> GetAllPeriods()
        {
            var allPeriods = this.dbContext.ElectricityMeasurements
                            .OrderByDescending(x => x.Id)
                            .Select(x => x.Period)
                            .Where(x => !x.StartsWith("Starting"))
                            .ToList();

            List<string> outputPeriods = new List<string>();
            List<SelectListItem> periodsViewModel = new List<SelectListItem>();

            foreach (var period in allPeriods)
            {
                if (!outputPeriods.Contains(period))
                {
                    outputPeriods.Add(period);
                    periodsViewModel.Add(new SelectListItem
                    {
                        Text = period,
                        Value = period
                    });
                }
            }

            return periodsViewModel.Take(12).ToList();
        }


        public void GenerateAccountingReport(AccountingReportViewModel input)
        {
            Tenant tenant = this.tenantsService.GetTenantById(input.TenantId);
            Landlord landlord = this.dbContext.Landlords.FirstOrDefault();
            PricesInformation pricesInformation = this.dbContext.PricesInformation.FirstOrDefault(x => x.Id == input.PricesInformationId);

            AccountingReport accountingReport = new AccountingReport
            {
                Number = input.Number,
                IssuedOn = input.CreatedOn,
                Tenant = tenant,
                Landlord = landlord,
                PricesInformation = pricesInformation,
                Period = input.Period,
                PricesInformationId = input.PricesInformationId,
                LandlordId = input.Landlord.Id,
                TenantId = input.TenantId,
                DayTimeElectricityConsummation = input.DayTimeElectricityConsummation,
                NightTimeElectricityConsummation = input.NightTimeElectricityConsummation,
                HeatingConsummation = input.HeatingConsummation,
                CoolingConsummation = input.CoolingConsummation,
                AmountForElectricity = input.AmountForElectricity,
                AmountForHeating = input.AmountForHeating,
                AmountForCooling = input.AmountForCooling,
                TotalAmount = input.TotalAmount,
            };
            
            tenant.AccountingReports.Add(accountingReport);
            this.dbContext.AccountingReports.Add(accountingReport);
            this.dbContext.SaveChanges();
        }

        public AccountingReportViewModel GetAccountingReportById(int accountingReportId)
        {
            var landlord = this.landlordsService.GetLandlord();
            var currentAccountingReport = this.dbContext.AccountingReports.Include(y=>y.Tenant).FirstOrDefault(x => x.Id == accountingReportId);
            var tenant = this.tenantsService.GetTenantByCompanyName(currentAccountingReport.Tenant.CompanyName);
            var currentTenantInfo = GetTenantInfo(tenant);
            var pricesInformation = this.pricesInformationService.GetPricesInformationById(currentAccountingReport.PricesInformationId);

            AccountingReportViewModel accountingReport = new AccountingReportViewModel
            {
                Number = currentAccountingReport.Number,
                Landlord = landlord,
                Tenant = currentTenantInfo,
                PricesInformation = pricesInformation,
                CreatedOn = currentAccountingReport.IssuedOn,
                Period = currentAccountingReport.Period,
                DayTimeElectricityConsummation = currentAccountingReport.DayTimeElectricityConsummation,
                NightTimeElectricityConsummation = currentAccountingReport.NightTimeElectricityConsummation,
                HeatingConsummation = currentAccountingReport.HeatingConsummation,
                CoolingConsummation = currentAccountingReport.CoolingConsummation,
                AmountForElectricity = currentAccountingReport.AmountForElectricity,
                AmountForHeating = currentAccountingReport.AmountForHeating,
                AmountForCooling = currentAccountingReport.AmountForCooling,
                TotalAmount = currentAccountingReport.TotalAmount
            };
            return accountingReport;
        }

        public IQueryable<AccountingReportListViewModel> GetAllAccountingReports()
        {
            var allAccountingReports = this.dbContext.AccountingReports.Select(x => new AccountingReportListViewModel
            {
                Id = x.Id,
                Number = x.Number,
                CompanyName = x.Tenant.CompanyName,
                CreatedOn = x.IssuedOn,
                //CreatedOn = x.IssuedOn.ToString("d.MM.yyyy") + " г.",

                Period = x.Period,
                //TotalAmount = x.TotalAmount.ToString("F2") + " лв."

                TotalAmount = x.TotalAmount
            });

            return allAccountingReports;
        }
               
        public TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(string tenantCompanyName, string period)
        {
            decimal tenantDayTimeElectricityConsummation = 0;
            decimal tenantNightTimeElectricityConsummation = 0;

            Tenant tenant = this.tenantsService.GetTenantByCompanyName(tenantCompanyName);


            foreach (var office in tenant.Offices)
            {
                var endOfPeriodElectricityMeasurement = this.dbContext.ElectricityMeasurements
                    .FirstOrDefault(x => x.ElectricityMeter.Office == office && x.Period == period);

                var startOfPeriodElectricityMeasurement = this.dbContext.ElectricityMeasurements
                    .Where(x => x.ElectricityMeterId == endOfPeriodElectricityMeasurement.ElectricityMeterId &&
                    x.Id < endOfPeriodElectricityMeasurement.Id)
                    .OrderByDescending(x => x.Id)
                    .First();

                decimal dayTimeOfficeConsumation =
                    endOfPeriodElectricityMeasurement.DayTimeMeasurement - startOfPeriodElectricityMeasurement.DayTimeMeasurement;

                decimal nightTimeOfficeConsumation =
                    endOfPeriodElectricityMeasurement.NightTimeMeasurement - startOfPeriodElectricityMeasurement.NightTimeMeasurement;

                tenantDayTimeElectricityConsummation += dayTimeOfficeConsumation;
                tenantNightTimeElectricityConsummation += nightTimeOfficeConsumation;
            }

            var tenantElectricityConsumation = new TenantElectricityConsummationViewModel
            {
                DayTimeConsummation = tenantDayTimeElectricityConsummation,
                NightTimeConsummation = tenantNightTimeElectricityConsummation
            };

            return tenantElectricityConsumation;
        }

        public TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period)
        {
            decimal tenantHeatingConsummation = 0;
            decimal tenantCoolingConsummation = 0;

            Tenant tenant = this.tenantsService.GetTenantByCompanyName(tenantCompanyName);

            foreach (var office in tenant.Offices)
            {
                var name = office.Name;

                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    var endOfPeriodTemperatureMeasurement = this.dbContext.TemperatureMeasurements
                        .FirstOrDefault(x => x.TemperatureMeter.Name == temperatureMeter.Name && x.Period == period);

                    var startOfPeriodTemperatureMeasurement = this.dbContext.TemperatureMeasurements
                        .Where(x => x.TemperatureMeterId == endOfPeriodTemperatureMeasurement.TemperatureMeterId &&
                        x.Id < endOfPeriodTemperatureMeasurement.Id)
                        .OrderByDescending(x => x.Id)
                        .First();

                    decimal heatingConsumation =
                        endOfPeriodTemperatureMeasurement.HeatingMeasurement - startOfPeriodTemperatureMeasurement.HeatingMeasurement;

                    decimal coolingConsumation =
                        endOfPeriodTemperatureMeasurement.CoolingMeasurement - startOfPeriodTemperatureMeasurement.CoolingMeasurement;

                    tenantHeatingConsummation += heatingConsumation;
                    tenantCoolingConsummation += coolingConsumation;
                }

            }

            var tenantTemperatureConsumation = new TenantTemperatureConsummationViewModel
            {
                HeatingConsummation = tenantHeatingConsummation,
                CoolingConsummation = tenantCoolingConsummation
            };

            return tenantTemperatureConsumation;
        }

        public decimal AmountForElectricity(TenantElectricityConsummationViewModel tenantElectricityConsummation)
        {
            var currentPrices = this.pricesInformationService.GetCurrentPrices();

            var consummation = tenantElectricityConsummation.DayTimeConsummation
                             + tenantElectricityConsummation.NightTimeConsummation;
            
            var price = (currentPrices.AccessToDistributionGrid
                       + currentPrices.NetworkTaxesAndUtilities
                       + currentPrices.Excise
                       + currentPrices.ElectricityPerKWh);

            var amountForElectricity = consummation * price;

            return amountForElectricity;
        }

        public AccountingReportViewModel GetAccountingReportViewModel(string tenantCompanyName, string period)
        {

            int number;
            if (this.dbContext.AccountingReports.Count() == 0)
            {
                number = 1;
            }
            else
            {
                number = this.dbContext.AccountingReports.Count() + 1;
            }

            var tenant = this.tenantsService.GetTenantByCompanyName(tenantCompanyName);

            var pricesInformation = this.pricesInformationService.GetCurrentPrices();

            var electricityConsummation = this.GetTenantElectricityConsummationByPeriod(tenant.CompanyName, period);
            var amountForElectricity = this.AmountForElectricity(electricityConsummation);
            var temperatureConsummation = this.GetTenantTemperatureConsummationByPeriod(tenant.CompanyName, period);

            var amountForHeating = temperatureConsummation.HeatingConsummation * pricesInformation.HeatingPerKWh;
            var amountForCooling = temperatureConsummation.CoolingConsummation * pricesInformation.CoolingPerKWh;
            var totalAmount = (amountForElectricity + amountForHeating + amountForCooling) * 1.20M;

            var tenantInfo = new TenantAccountingReportViewModel
            {
                Id = tenant.Id,
                CompanyName = tenant.CompanyName,
                Address = tenant.Address,
                Bulstat = tenant.Bulstat,
                EIK = this.tenantsService.GetTenantEIK(tenantCompanyName),
                CompanyOwner = tenant.CompanyOwner,
                StartOfContract = tenant.StartOfContract,
                Offices = this.tenantsService.GetTenantOfficesAsText(tenantCompanyName),
            };
            ;
            AccountingReportViewModel accountingReport = new AccountingReportViewModel
            {
                Number = number,
                Landlord = this.landlordsService.GetLandlord(),
                Tenant = tenantInfo,
                CreatedOn = DateTime.UtcNow,
                Period = period,
                PricesInformation = pricesInformation,
                DayTimeElectricityConsummation = electricityConsummation.DayTimeConsummation,
                NightTimeElectricityConsummation = electricityConsummation.NightTimeConsummation,
                AmountForElectricity = this.AmountForElectricity(electricityConsummation),
                HeatingConsummation = temperatureConsummation.HeatingConsummation,
                AmountForHeating = amountForHeating,
                CoolingConsummation = temperatureConsummation.CoolingConsummation,
                AmountForCooling = amountForCooling,
                PricesInformationId = pricesInformation.Id,
                TenantId = tenant.Id,
                TotalAmount = totalAmount,
            };

            return accountingReport;
        }
        private static TenantAccountingReportViewModel GetTenantInfo(Tenant tenant)
        {
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
            return tenantInfo;
        }
    }
}
