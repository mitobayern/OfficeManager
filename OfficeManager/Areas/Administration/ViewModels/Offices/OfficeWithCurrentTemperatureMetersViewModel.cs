namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.Collections.Generic;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;

    public class OfficeWithCurrentTemperatureMetersViewModel
    {
        public int Id { get; set; }

        public IEnumerable<TemperatureMeterOutputViewModel> CurrentTemperatureMeters { get; set; }
    }
}
