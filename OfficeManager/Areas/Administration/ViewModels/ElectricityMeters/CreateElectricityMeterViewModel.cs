namespace OfficeManager.Areas.Administration.ViewModels.ElectricityMeters
{
    using System.ComponentModel.DataAnnotations;

    public class CreateElectricityMeterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal PowerSupply { get; set; }
    }
}
