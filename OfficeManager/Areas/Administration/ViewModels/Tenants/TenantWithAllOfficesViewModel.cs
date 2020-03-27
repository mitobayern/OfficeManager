namespace OfficeManager.Areas.Administration.ViewModels.Tenants
{
    using System.Collections.Generic;
    using OfficeManager.Areas.Administration.ViewModels.Offices;

    public class TenantWithAllOfficesViewModel
    {
        public int Id { get; set; }

        public IEnumerable<EditOfficeViewModel> AvailavleOffices { get; set; }

        public IEnumerable<EditOfficeViewModel> CurrentOffices { get; set; }
    }
}
