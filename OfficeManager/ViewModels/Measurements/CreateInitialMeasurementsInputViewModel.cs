namespace OfficeManager.ViewModels.Measurements
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public class CreateInitialMeasurementsInputViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndOfPeriod { get; set; }

        [BindProperty]
        public List<OfficeMeasurementsInputViewModel> Offices { get; set; }
    }
}
