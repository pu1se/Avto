using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentMS.BL
{
    public class NotEmptyValueRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var type = value.GetType();
            if (!type.IsValueType)
            {
                return null;
            }

            if (object.Equals(value, GetDefaultValue(type)))
            {
                return new ValidationResult($"This field must be in valid format and not equal to default type value", new List<String>() { validationContext.DisplayName });
            }
            return null;
        }

        private static object GetDefaultValue(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
