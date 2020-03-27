namespace OfficeManager.Areas.Administration.ViewModels.PricesInformation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CreatePricesInputViewModel
    {
        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal HeatingPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal CoolingPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ElectricityPerKWh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal Excise { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal AccessToDistributionGrid { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal NetworkTaxesAndUtilities { get; set; }
    }
}
