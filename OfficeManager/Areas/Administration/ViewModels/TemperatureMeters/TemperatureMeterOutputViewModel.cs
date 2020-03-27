namespace OfficeManager.Areas.Administration.ViewModels.TemperatureMeters
{
    using System.ComponentModel.DataAnnotations;

    public class TemperatureMeterOutputViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string OfficeNumber { get; set; }
    }
}
