namespace OfficeManager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TemperatureMeasurement
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HeatingMeasurement { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CoolingMeasurement { get; set; }

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

        public int TemperatureMeterId { get; set; }
        public virtual TemperatureMeter TemperatureMeter { get; set; }
    }
}
