namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.Collections.Generic;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;

    public class OfficeWithAllTemperatureMetersViewModel
    {
        public int Id { get; set; }

        public IEnumerable<TemperatureMeterOutputViewModel> AvailavleTemperatureMeters { get; set; }
    }
}
