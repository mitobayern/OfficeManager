namespace OfficeManager.ViewModels.Measurements
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public class CreateMeasurementsInputViewModel : IValidatableObject
    {
        [BindProperty]
        public string LastPeriod { get; set; }

        public DateTime EndOfLastPeriod { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartOfPeriod { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndOfPeriod { get; set; }

        [BindProperty]
        public List<OfficeMeasurementsInputViewModel> Offices { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Compare(this.StartOfPeriod, this.EndOfLastPeriod) != 1)
            {
                yield return new ValidationResult($"Start of period must be after {this.EndOfLastPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))} г.", new List<string> { "StartOfPeriod" });

                if (DateTime.Compare(this.EndOfPeriod, this.EndOfLastPeriod) != 1)
                {
                    yield return new ValidationResult($"End of period must be after {this.EndOfLastPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))} г.", new List<string> { "EndOfPeriod" });
                }
            }

            if (DateTime.Compare(this.EndOfPeriod, this.StartOfPeriod) != 1)
            {
                yield return new ValidationResult($"End of period must be after {this.StartOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))} г.", new List<string> { "EndOfPeriod" });
            }
        }
    }
}
