using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.Parser._Core.CallResults
{
    public static class Result
    {
        public static CallResult Success()
        {
            return new CallResult();
        }

        public static CallResult<T> Success<T>(T result)
        {
            return new CallResult<T>(result);
        }

        public static CallListResult<T> SuccessList<T>(IEnumerable<T> result)
        {
            return new CallListResult<T>(result);
        }

        public static CallResult Fail(CallResult callResult)
        {
            return callResult;
        }

        public static CallResult Fail(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallResult(errorMessage, errorType);
        }

        public static CallResult<T> Fail<T>(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallResult<T>(errorMessage, errorType);
        }

        public static CallListResult<T> FailList<T>(
            string errorMessage,
            ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallListResult<T>(errorMessage, errorType);
        }

        public static CallResult<T> Fail<T>(CallResult callResult)
        {
            return new CallResult<T>(callResult);
        }

        public static NotFoundResult NotFound(string errorMessage)
        {
            return new NotFoundResult(errorMessage);
        }

        public static NotFoundResult<T> NotFound<T>(string errorMessage)
        {
            return new NotFoundResult<T>(errorMessage);
        }

        public static CallResult ValidationFail(IRequestModelWithValidation model)
        {
            return new ValidationFailResult(model) as CallResult;
        }

        public static CallResult<T> ValidationFail<T>(IRequestModelWithValidation model)
        {
            return new ValidationFailResult<T>(model);
        }

        public static CallListResult<T> ValidationFailList<T>(IRequestModelWithValidation model)
        {
            return new ValidationFailListResult<T>(model);
        }

        public static CallResult ValidationFail(Dictionary<string, string> modelErrors)
        {
            return new ValidationFailResult(modelErrors);
        }

        public static CallResult ValidationFail(string key, string errorMessage)
        {
            return new ValidationFailResult(key, errorMessage);
        }

        public static CallResult<T> ValidationFail<T>(string key, string errorMessage)
        {
            return new ValidationFailResult<T>(key, errorMessage);
        }

        public static CallResult SuccessValidation()
        {
            return new CallResult();
        }

        public static CallResult<T> SuccessValidation<T>() where T : new()
        {
            return new CallResult<T>(new T());
        }

        public static CallListResult<T> SuccessValidationList<T>()
        {
            return new CallListResult<T>(new List<T>());
        }
    }
}
