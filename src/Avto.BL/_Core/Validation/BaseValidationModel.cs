using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Avto.BL
{
    public abstract class BaseValidationModel : IRequestModelWithValidation
    {
        private Dictionary<string, List<string>> _validationErrors;

        public virtual Dictionary<string, List<string>> GetValidationErrors()
        {
            if (_validationErrors == null)
                this.IsValid();

            return _validationErrors;
        }

        public bool CheckValidation()
        {
            _validationErrors = new Dictionary<string, List<string>>();
            var validationResultList = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            var isValid = Validator.TryValidateObject(this, validationContext, validationResultList, true);

            foreach (var error in validationResultList)
            {
                foreach (var errorName in error.MemberNames)
                {
                    if (!_validationErrors.ContainsKey(errorName))
                    {
                        _validationErrors[errorName] = new List<string>();
                    }

                    _validationErrors[errorName].Add(error.ErrorMessage);
                }
            }

            return isValid;
        }
    }

    public static class BaseValidationExtensions
    {
        public static bool IsValid(this BaseValidationModel model)
        {
            if (model == null)
            {
                return false;
            }

            return model.CheckValidation();
        }
    }
}
