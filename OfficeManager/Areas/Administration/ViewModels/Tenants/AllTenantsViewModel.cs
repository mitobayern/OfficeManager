namespace OfficeManager.Areas.Administration.ViewModels.Tenants
{
    using System.Collections.Generic;

    public class AllTenantsViewModel
    {
        public IEnumerable<TenantOutputViewModel> Tenants { get; set; }
    }
}
