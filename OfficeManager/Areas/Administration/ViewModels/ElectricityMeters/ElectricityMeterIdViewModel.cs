namespace OfficeManager.Areas.Administration.ViewModels.ElectricityMeters
{
    using System.ComponentModel.DataAnnotations;

    public class ElectricityMeterIdViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
