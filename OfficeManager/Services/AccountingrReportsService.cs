using Microsoft.EntityFrameworkCore;
using OfficeManager.Data;
using OfficeManager.Models;
using OfficeManager.ViewModels.AccountingReports;
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
            ;
            tenant.AccountingReports.Add(accountingReport);
            this.dbContext.AccountingReports.Add(accountingReport);
            this.dbContext.SaveChanges();
        }

        public AccountingReportViewModel GetAccountingReportById(int accountingReportId)
        {
            var landlord = this.landlordsService.GetLandlord();
            var currentAccountingReport = this.dbContext.AccountingReports.Include(y=>y.Tenant).FirstOrDefault(x => x.Id == accountingReportId);
            var tenant = this.tenantsService.GetTenantByName(currentAccountingReport.Tenant.CompanyName);
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
                CreatedOn = x.IssuedOn.ToString("d.MM.yyyy") + " г.",
                Period = x.Period,
                TotalAmount = x.TotalAmount.ToString("F2") + " лв."
            });

            return allAccountingReports;
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
