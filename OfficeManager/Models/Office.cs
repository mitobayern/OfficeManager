namespace OfficeManager.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Office
    {
        public Office()
        {
            this.IsAvailable = true;
            this.TemperatureMeters = new HashSet<TemperatureMeter>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Area { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RentPerSqMeter { get; set; }

        public decimal TotalRent => this.Area * this.RentPerSqMeter;

        public bool IsAvailable { get; set; }

        public bool IsDeleted { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ElectricityMeter ElectricityMeter { get; set; }

        public virtual ICollection<TemperatureMeter> TemperatureMeters { get; set; }
    }
}
