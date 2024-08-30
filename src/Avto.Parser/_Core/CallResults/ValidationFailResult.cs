using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avto.Parser._Core.CallResults
{
    public class ValidationFailResult : CallResult
    {
        public ValidationFailResult(IRequestModelWithValidation model)
        {
            if (model == null)
            {
                ErrorType = ErrorType.ModelWasNotInitialized400;
                ValidationErrors.Add("model", new List<string> { "Can not parse parameters from request" });
            }

            var modelErrors = model?.GetValidationErrors();
            if (modelErrors?.Count > 0)
            {
                ErrorType = ErrorType.ValidationError400;
                ErrorMessage = "Validation error";
                ValidationErrors = modelErrors;
            }
        }

        public ValidationFailResult(string key, string errorMessage)
        {
            var modelErrors = new Dictionary<string, List<string>>
            {
                {key, new List<string>{errorMessage}}
            };

            ErrorType = ErrorType.ValidationError400;
            ErrorMessage = "Validation error";
            ValidationErrors = modelErrors;
        }

        public ValidationFailResult(Dictionary<string, string> modelErrors)
        {
            foreach (var item in modelErrors)
            {
                ValidationErrors[item.Key] = new List<string> { item.Value };
            }

            ErrorType = ErrorType.ValidationError400;
            ErrorMessage = "Validation error";
        }
    }

    public class ValidationFailResult<T> : CallResult<T>
    {
        public ValidationFailResult(IRequestModelWithValidation model) : base(default(T))
        {
            if (model == null)
            {
                ErrorType = ErrorType.ModelWasNotInitialized400;
                ValidationErrors.Add("model", new List<string> { "Can not parse parameters from request" });
            }

            var modelErrors = model?.GetValidationErrors();
            if (modelErrors?.Count > 0)
            {
                ErrorType = ErrorType.ValidationError400;
                ErrorMessage = "Validation error";
                ValidationErrors = modelErrors;
            }
        }

        public ValidationFailResult(string key, string errorMessage) : base(default(T))
        {
            var modelErrors = new Dictionary<string, List<string>>
            {
                {key, new List<string>{errorMessage}}
            };

            ErrorType = ErrorType.ValidationError400;
            ErrorMessage = "Validation error";
            ValidationErrors = modelErrors;
        }
    }


    public class ValidationFailListResult<T> : CallListResult<T>
    {
        public ValidationFailListResult(IRequestModelWithValidation model)
            : base(default)
        {
            if (model == null)
            {
                ErrorType = ErrorType.ModelWasNotInitialized400;
                ValidationErrors.Add("model", new List<string> { "Can not parse parameters from request" });
            }

            var modelErrors = model?.GetValidationErrors();
            if (modelErrors?.Count > 0)
            {
                ErrorType = ErrorType.ValidationError400;
                ErrorMessage = "Validation error";
                ValidationErrors = modelErrors;
            }
        }

        public ValidationFailListResult(string key, string errorMessage)
            : base(default)
        {
            var modelErrors = new Dictionary<string, List<string>>
            {
                {key, new List<string>{errorMessage}}
            };

            ErrorType = ErrorType.ValidationError400;
            ErrorMessage = "Validation error";
            ValidationErrors = modelErrors;
        }
    }
}
