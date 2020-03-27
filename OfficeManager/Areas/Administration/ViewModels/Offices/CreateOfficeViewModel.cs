namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOfficeViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Area { get; set; }

        [Required]
        public decimal RentPerSqMeter { get; set; }
    }
}
