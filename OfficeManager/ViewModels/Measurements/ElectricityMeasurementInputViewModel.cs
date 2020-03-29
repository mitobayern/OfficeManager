using System.ComponentModel.DataAnnotations;

namespace OfficeManager.ViewModels.Measurements
{
    public class ElectricityMeasurementInputViewModel
    {
        public string Name { get; set; }

        [Required]
        public decimal DayTimeMeasurement { get; set; }
       
        [Required]
        public decimal NightTimeMeasurement { get; set; }
    }
}
