namespace OfficeManager.ViewModels.Measurements
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ElectricityMeasurementInputViewModel : IValidatableObject
    {
        [Required]
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

