namespace OfficeManager.ViewModels.AccountingReports
{
    using System;

    public class AccountingReportListViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Period { get; set; }

        public string CompanyName { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
