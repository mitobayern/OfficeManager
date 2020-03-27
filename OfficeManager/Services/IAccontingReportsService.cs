using OfficeManager.ViewModels.AccountingReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public interface IAccontingReportsService
    {
        void GenerateAccountingReport(AccountingReportViewModel accountingReport);
        IQueryable<AccountingReportListViewModel> GetAllAccountingReports();

        AccountingReportViewModel GetAccountingReportById(int аccountingReportId);
    }
}
