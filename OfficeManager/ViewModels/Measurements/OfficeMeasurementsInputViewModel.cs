namespace OfficeManager.ViewModels.Measurements
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OfficeMeasurementsInputViewModel
    {
        [Required]
        public string Name { get; set; }

        public ElectricityMeasurementInputViewModel ElectricityMeter { get; set; }

        public List<TemperatureMeasurementInputViewModel> TemperatureMeters { get; set; }
    }
}
