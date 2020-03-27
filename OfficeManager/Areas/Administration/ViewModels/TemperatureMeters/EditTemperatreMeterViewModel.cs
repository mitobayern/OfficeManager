namespace OfficeManager.Areas.Administration.ViewModels.TemperatureMeters
{
    using System.ComponentModel.DataAnnotations;

    public class EditTemperatreMeterViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string OfficeName { get; set; }
    }
}
