using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OfficeManager.ViewModels.Measurements
{
    public class TemperatureMeasurementInputViewModel :IValidatableObject
    {
        public string Name { get; set; }

        [Required]
        public decimal HeatingMinValue { get; set; }

        [Required]
        public decimal CoolingMinValue { get; set; }

        [Required]
        public decimal HeatingMeasurement { get; set; }

        [Required]
        public decimal CoolingMeasurement { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.HeatingMeasurement < this.HeatingMinValue)
            {
                yield return new ValidationResult($"Measurement can not be smaller than {this.HeatingMinValue}", new List<string> { "HeatingMeasurement" });
            }

            if (this.CoolingMeasurement < this.CoolingMinValue)
            {
                yield return new ValidationResult($"Measurement can not be smaller than {this.CoolingMinValue}", new List<string> { "CoolingMeasurement" });
            }
        }


    }
}
