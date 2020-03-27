namespace OfficeManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tenant
    {
        public Tenant()
        {
            this.Offices = new HashSet<Office>();
            this.Invoices = new HashSet<Invoice>();
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

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartOfContract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndOfContract { get; set; }

        public bool hasContract { get; set; }

        public virtual ICollection<Office> Offices { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<AccountingReport> AccountingReports { get; set; }

    }
}
