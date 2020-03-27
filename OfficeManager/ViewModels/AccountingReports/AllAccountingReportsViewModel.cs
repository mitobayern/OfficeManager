namespace OfficeManager.ViewModels.AccountingReports
{
    using System.Collections.Generic;

    public class AllAccountingReportsViewModel
    {
        public IEnumerable<AccountingReportListViewModel> AccountingReports { get; set; }
    }
}
