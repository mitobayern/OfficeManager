namespace OfficeManager.ViewModels.AccountingReports
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

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
            if (this.AccountingReports != null)
            {
                if (this.AccountingReports.Any(x => x.CompanyName == this.Tenant && x.Period == this.Period))
                {
                    yield return new ValidationResult($"{this.Tenant} already has accounting report for {this.Period}", new List<string> { "Tenant" });
                }
            }
        }
    }
}
