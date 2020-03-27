namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditOfficeViewModel
    {
        public int Id { get; set; }

        public bool isChecked => false;

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Area { get; set; }

        [Required]
        public decimal RentPerSqMeter { get; set; }

        public string ElectricityMeter { get; set; }

        public IEnumerable<string> AllElecticityMeters { get; set; }

        public IEnumerable<string> TemperatureMeters { get; set; }

        public IEnumerable<string> AllTemperatureMeters { get; set; }
    }
}
