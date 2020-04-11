using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
using OfficeManager.ViewModels.AccountingReports;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public interface IAccontingReportsService
    {
        List<SelectListItem> GetAllPeriodsSelectList();

        List<SelectListItem> GetAllTenantsSelectList();

        List<string> AllPeriods();

        List<string> AlTenants();

        TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(string tenantCompanyName, string period);

        TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period);

        decimal AmountForElectricity(TenantElectricityConsummationViewModel tenantElectricityConsummation);

        void GenerateAccountingReport(AccountingReportViewModel accountingReport);

        IQueryable<AccountingReportListViewModel> GetAllAccountingReports();

        AccountingReportViewModel GetAccountingReportById(int аccountingReportId);

        AccountingReportViewModel GetAccountingReportViewModel(string tenantCompanyName, string period);
    }
}
