namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    using System.Collections.Generic;

    public class AllOfficesViewModel
    {
        public IEnumerable<OfficeOutputViewModel> Offices { get; set; }
    }
}
