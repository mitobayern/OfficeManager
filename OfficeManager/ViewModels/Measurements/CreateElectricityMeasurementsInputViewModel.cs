using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.ViewModels.Measurements
{
    public class CreateElectricityMeasurementsInputViewModel
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
        public List<ElectricityMeasurementInputViewModel> ElectricityMeters { get; set; }
    }
}
