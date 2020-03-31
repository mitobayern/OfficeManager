using OfficeManager.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OfficeManager.ViewModels.Measurements
{
    public class ElectricityMeasurementInputViewModel : IValidatableObject
    {
        public string Name { get; set; }

        [Required]
        public decimal DayTimeMinValue { get; set; }

        [Required]
        public decimal NightTimeMinValue { get; set; }

        [Required]
        public decimal DayTimeMeasurement { get; set; }

        [Required]
        public decimal NightTimeMeasurement { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DayTimeMeasurement < this.DayTimeMinValue)
            {
                yield return new ValidationResult($"Measurement can not be smaller than {this.DayTimeMinValue}", new List<string> {"DayTimeMeasurement"});
            }

            if (this.NightTimeMeasurement < this.NightTimeMinValue)
            {
                yield return new ValidationResult($"Measurement can not be smaller than {this.NightTimeMinValue}", new List<string> { "NightTimeMeasurement" });
            }
        }
    }
}

