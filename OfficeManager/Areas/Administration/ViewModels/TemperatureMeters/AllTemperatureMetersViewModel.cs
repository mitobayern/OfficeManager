namespace OfficeManager.Areas.Administration.ViewModels.TemperatureMeters
{
    using System.Collections.Generic;

    public class AllTemperatureMetersViewModel
    {
        public IEnumerable<TemperatureMeterOutputViewModel> TemperatureMeters { get; set; }
    }
}
