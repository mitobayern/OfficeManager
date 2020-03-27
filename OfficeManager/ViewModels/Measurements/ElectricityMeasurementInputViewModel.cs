namespace OfficeManager.ViewModels.Measurements
{
    public class ElectricityMeasurementInputViewModel
    {
        public string Name { get; set; }

        public decimal DayTimeMeasurement { get; set; }
       
        public decimal NightTimeMeasurement { get; set; }
    }
}
