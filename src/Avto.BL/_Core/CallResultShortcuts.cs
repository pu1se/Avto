using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avto.BL
{
    //todo: remove call results
    public abstract class CallResultShortcuts
    {
        protected CallResult SuccessResult()
        {
            return Result.Success();
        }

        protected CallDataResult<T> SuccessResult<T>(T result)
        {
            return Result.Success(result);
        }

        protected CallListDataResult<T> SuccessListResult<T>(IEnumerable<T> result)
        {
            return Result.SuccessList(result);
        }

        protected CallResult FailResult(CallResult callResult)
        {
            return Result.Fail(callResult);
        }

        protected CallResult FailResult(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return Result.Fail(errorMessage, errorType);
        }

        protected CallDataResult<T> FailResult<T>(string errorMessage, ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return Result.Fail<T>(errorMessage, errorType);
        }

        protected CallListDataResult<T> FailListResult<T>(
            string errorMessage, 
            ErrorType errorType = ErrorType.UnexpectedError500)
        {
            return Result.FailList<T>(errorMessage, errorType);
        }

        protected CallListDataResult<T> FailListResult<T>(CallResult callResult)
        {
            return Result.FailList<T>(callResult.ErrorMessage, callResult.ErrorType);
        }

        protected CallDataResult<T> FailResult<T>(CallResult callResult)
        {
            return Result.Fail<T>(callResult);
        }

        protected NotFoundResult NotFoundResult(string errorMessage)
        {
            return Result.NotFound(errorMessage);
        }

        protected NotFoundResult<T> NotFoundResult<T>(string errorMessage)
        {
            return Result.NotFound<T>(errorMessage);
        }

        protected CallResult ValidationFailResult(IValidationModel model)
        {
            return Result.ValidationFail(model);
        }

        protected CallDataResult<T> ValidationFailResult<T>(IValidationModel model)
        {
            return Result.ValidationFail<T>(model);
        }

        protected CallListDataResult<T> ValidationFailListResult<T>(IValidationModel model)
        {
            return Result.ValidationFailList<T>(model);
        }

        protected CallResult ValidationFailResult(Dictionary<string, string> modelErrors)
        {
            return Result.ValidationFail(modelErrors);
        }

        protected CallResult ValidationFailResult(string key, string errorMessage)
        {
            return Result.ValidationFail(key, errorMessage);
        }

        protected CallDataResult<T> ValidationFailResult<T>(string key, string errorMessage)
        {
            return Result.ValidationFail<T>(key, errorMessage);
        }

        protected CallResult ValidationSuccessResult()
        {
            return Result.SuccessValidation();
        }

        protected CallDataResult<T> ValidationSuccessResult<T>() where T : new()
        {
            return Result.SuccessValidation<T>();
        }

        protected CallListDataResult<T> ValidationSuccessListResult<T>()
        {
            return Result.SuccessValidationList<T>();
        }
    }
}
