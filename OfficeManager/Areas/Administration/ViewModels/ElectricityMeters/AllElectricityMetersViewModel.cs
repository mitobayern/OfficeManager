namespace OfficeManager.Areas.Administration.ViewModels.ElectricityMeters
{
    using System.Collections.Generic;

    public class AllElectricityMetersViewModel
    {
        public IEnumerable<ElectricityMeterOutputViewModel> ElectricityMeters { get; set; }
    }
}
