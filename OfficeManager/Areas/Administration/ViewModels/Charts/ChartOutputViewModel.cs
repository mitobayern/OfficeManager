namespace OfficeManager.Areas.Administration.ViewModels.Charts
{
    public class ChartOutputViewModel
    {
        public string Period { get; set; }

        public decimal AmountForElectricity { get; set; }

        public decimal AmountForHeating { get; set; }

        public decimal AmountForCooling { get; set; }
    }
}
