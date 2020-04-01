namespace OfficeManager.ViewModels.Measurements
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
