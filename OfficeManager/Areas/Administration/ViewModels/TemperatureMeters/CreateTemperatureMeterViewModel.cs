namespace OfficeManager.Areas.Administration.ViewModels.TemperatureMeters
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTemperatureMeterViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
