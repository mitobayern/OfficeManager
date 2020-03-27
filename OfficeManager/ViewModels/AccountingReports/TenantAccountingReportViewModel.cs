namespace OfficeManager.ViewModels.AccountingReports
{
    using System;

    public class TenantAccountingReportViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CompanyOwner { get; set; }

        public string EIK { get; set; }

        public string Bulstat { get; set; }

        public string Address { get; set; }

        public DateTime StartOfContract { get; set; }

        public string Offices { get; set; }

        
    }
}
