namespace OfficeManager.ViewModels.AccountingReports
{
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Areas.Administration.ViewModels.PricesInformation;
    using System;

    public class AccountingReportViewModel
    {
        public int Number { get; set; }

        public LandlordOutputViewModel Landlord { get; set; }

        public int TenantId { get; set; }
        public TenantAccountingReportViewModel Tenant { get; set; }

        public string Period { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PricesInformationId { get; set; }
        public CurrentPricesOutputViewModel PricesInformation { get; set; }

        public decimal DayTimeElectricityConsummation { get; set; }

        public decimal NightTimeElectricityConsummation { get; set; }

        public decimal AmountForElectricity { get; set; }

        public decimal HeatingConsummation { get; set; }

        public decimal AmountForHeating { get; set; }

        public decimal CoolingConsummation { get; set; }

        public decimal AmountForCooling { get; set; }

        public decimal AmountForCleaning { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
