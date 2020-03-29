using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.ViewModels.Measurements
{
    public class OfficeMeasurementsInputViewModel
    {
        public string Name { get; set; }

        public ElectricityMeasurementInputViewModel ElectricityMeter { get; set; }

        public List<TemperatureMeasurementInputViewModel> TemperatureMeters { get; set; }

    }
}
