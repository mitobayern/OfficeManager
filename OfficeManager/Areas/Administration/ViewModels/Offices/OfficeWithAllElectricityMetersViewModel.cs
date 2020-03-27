namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.Collections.Generic;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;

    public class OfficeWithAllElectricityMetersViewModel
    {
        public int Id { get; set; }

        public IEnumerable<ElectricityMeterOutputViewModel> AvailavleElectricityMeters { get; set; }
    }
}
