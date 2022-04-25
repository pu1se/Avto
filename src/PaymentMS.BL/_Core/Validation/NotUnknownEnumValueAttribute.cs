using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentMS.BL.Validation
{
    public class NotUnknownEnumValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if ((int)value == 0)
            {
                return new ValidationResult($"This enum value can not be unknown", new List<String>() { validationContext.DisplayName });
            }
            return null;
        }
    }
}
