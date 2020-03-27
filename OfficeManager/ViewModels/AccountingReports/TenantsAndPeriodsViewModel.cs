namespace OfficeManager.ViewModels.AccountingReports
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class TenantsAndPeriodsViewModel
    {
        public List<SelectListItem> Periods { get; set; }

        public List<SelectListItem> Tenants { get; set; }

        public string Period { get; set; }

        public string Tenant { get; set; }
    }
}
