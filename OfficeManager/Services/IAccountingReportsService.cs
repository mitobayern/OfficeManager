namespace OfficeManager.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OfficeManager.ViewModels.AccountingReports;
    using OfficeManager.ViewModels.Measurements;

    public interface IAccountingReportsService
    {
        Task GenerateAccountingReportAsync(AccountingReportViewModel accountingReport);

        List<SelectListItem> GetAllPeriodsSelectList();

        List<SelectListItem> GetAllTenantsSelectList();

        List<string> AllPeriods();

        List<string> AlTenants();

        TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(string tenantCompanyName, string period);

        TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period);

        decimal AmountForElectricity(TenantElectricityConsummationViewModel tenantElectricityConsummation);

        IQueryable<AccountingReportListViewModel> GetAllAccountingReports();

        AccountingReportViewModel GetAccountingReportById(int аccountingReportId);

        AccountingReportViewModel GetAccountingReportViewModel(string tenantCompanyName, string period);
    }
}
