namespace OfficeManager.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ElectricityMeter
    {
        public ElectricityMeter()
        {
            this.ElectricityMeasurements = new HashSet<ElectricityMeasurement>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PowerSupply { get; set; }

        public int? OfficeId { get; set; }

        public virtual Office Office { get; set; }

        public virtual ICollection<ElectricityMeasurement> ElectricityMeasurements { get; set; }
    }
}
