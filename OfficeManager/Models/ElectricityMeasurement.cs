namespace OfficeManager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ElectricityMeasurement
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DayTimeMeasurement { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NightTimeMeasurement { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartOfPeriod { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndOfPeriod { get; set; }

        [Required]
        public string Period { get; set; }

        public int ElectricityMeterId { get; set; }

        public virtual ElectricityMeter ElectricityMeter { get; set; }
    }
}
