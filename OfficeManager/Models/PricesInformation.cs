namespace OfficeManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PricesInformation
    {
        public PricesInformation()
        {
            this.AccountingReports = new HashSet<AccountingReport>();
        }

        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal HeatingPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal CoolingPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal ElectricityPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal Excise { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal AccessToDistributionGrid { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal NetworkTaxesAndUtilities { get; set; }

        public virtual ICollection<AccountingReport> AccountingReports { get; set; }
    }
}
