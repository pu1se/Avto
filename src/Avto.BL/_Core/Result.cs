using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL
{
    public static class Result
    {
        public static CallResult Success()
        {
            return new CallResult();
        }

        public static CallDataResult<T> Success<T>(T result)
        {
            return new CallDataResult<T>(result);
        }

        public static CallListDataResult<T> SuccessList<T>(IEnumerable<T> result)
        {
            return new CallListDataResult<T>(result);
        }

        public static CallResult Fail(CallResult callResult)
        {
            return callResult;
        }

        public static CallResult Fail(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallResult(errorMessage, errorType);
        }

        public static CallDataResult<T> Fail<T>(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallDataResult<T>(errorMessage, errorType);
        }

        public static CallListDataResult<T> FailList<T>(
            string errorMessage, 
            ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return new CallListDataResult<T>(errorMessage, errorType);
        }

        public static CallDataResult<T> Fail<T>(CallResult callResult)
        {
            return new CallDataResult<T>(callResult);
        }

        public static NotFoundResult NotFound(string errorMessage)
        {
            return new NotFoundResult(errorMessage);
        }

        public static NotFoundResult<T> NotFound<T>(string errorMessage)
        {
            return new NotFoundResult<T>(errorMessage);
        }

        public static CallResult ValidationFail(IValidationModel model)
        {
            return new ValidationFailResult(model) as CallResult;
        }

        public static CallDataResult<T> ValidationFail<T>(IValidationModel model)
        {
            return new ValidationFailResult<T>(model);
        }

        public static CallListDataResult<T> ValidationFailList<T>(IValidationModel model)
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

        public static CallDataResult<T> ValidationFail<T>(string key, string errorMessage)
        {
            return new ValidationFailResult<T>(key, errorMessage);
        }

        public static CallResult SuccessValidation()
        {
            return new CallResult();
        }

        public static CallDataResult<T> SuccessValidation<T>() where T : new()
        {
            return new CallDataResult<T>(new T());
        }

        public static CallListDataResult<T> SuccessValidationList<T>()
        {
            return new CallListDataResult<T>(new List<T>());
        }
    }
}
