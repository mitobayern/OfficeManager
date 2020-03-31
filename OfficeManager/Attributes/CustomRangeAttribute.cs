using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Attributes
{
    public class CustomRangeAttribute : ValidationAttribute, IValidatableObject
    {
        public CustomRangeAttribute(decimal minValue, decimal inputValue)
        {
            MinValue = minValue;
            InputValue = inputValue;
        }

        [Range(1, 5)]
        public decimal MinValue { get; set; }

        [Range(1, 5)]
        public decimal InputValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }

}
}
