namespace OfficeManager.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Landlord
    {
        public Landlord()
        {
            this.AccountingReports = new HashSet<AccountingReport>();
        }

        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyOwner { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Bulstat { get; set; }

        public virtual ICollection<AccountingReport> AccountingReports { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
