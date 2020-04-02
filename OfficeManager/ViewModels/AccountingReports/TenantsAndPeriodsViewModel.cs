namespace OfficeManager.ViewModels.AccountingReports
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TenantsAndPeriodsViewModel : IValidatableObject
    {
        public List<SelectListItem> Periods { get; set; }

        public List<SelectListItem> Tenants { get; set; }

        [Required]
        public string Period { get; set; }

        [Required]
        public string Tenant { get; set; }

        [BindProperty]
        public List<AccountingReportListViewModel> AccountingReports { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccountingReports != null)
            {
                if (AccountingReports.Any(x => x.CompanyName == Tenant && x.Period == Period))
                {
                    yield return new ValidationResult($"{Tenant} already has accounting report for {Period}", new List<string> { "Tenant" });
                }
            }
        }
    }
}
