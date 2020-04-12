namespace OfficeManager.Areas.Administration.ViewModels.PricesInformation
{
    public class CurrentPricesOutputViewModel
    {
        public int Id { get; set; }

        public string CreatedOn { get; set; }

        public decimal HeatingPerKWh { get; set; }

        public decimal CoolingPerKWh { get; set; }

        public decimal ElectricityPerKWh { get; set; }

        public decimal Excise { get; set; }

        public decimal AccessToDistributionGrid { get; set; }

        public decimal NetworkTaxesAndUtilities { get; set; }
    }
}
