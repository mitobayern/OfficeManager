namespace OfficeManager.Areas.Administration.ViewModels.ElectricityMeters
{
    using System.ComponentModel.DataAnnotations;

    public class ElectricityMeterOutputViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal PowerSupply { get; set; }

        public string OfficeNumber { get; set; }
    }
}
