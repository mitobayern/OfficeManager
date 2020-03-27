namespace OfficeManager.ViewModels.Measurements
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateTemperatureMeasurementsInputViewModel
    {
        [BindProperty]
        public string LastPeriod { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StarOfPeriod { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndOfPeriod { get; set; }

        [BindProperty]
        public List<TemperatureMeasurementInputViewModel> TemperatureMeters { get; set; }
    }
}
