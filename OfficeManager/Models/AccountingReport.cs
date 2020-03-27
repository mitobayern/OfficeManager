namespace OfficeManager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AccountingReport
    {
        public int Id { get; set; }

        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }

        public int LandlordId { get; set; }
        public virtual Landlord Landlord { get; set; }

        public int PricesInformationId { get; set; }
        public virtual PricesInformation PricesInformation { get; set; }

        public int Number { get; set; }

        public DateTime IssuedOn { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HeatingConsummation { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CoolingConsummation { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DayTimeElectricityConsummation { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NightTimeElectricityConsummation { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountForHeating { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountForCooling { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountForElectricity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountForCleaning { get; set; }

        public string Period { get; set; }
        
    }
}
