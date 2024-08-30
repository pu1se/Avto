
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avto.Parser._Core.CallResults
{
    public class CallResult
    {
        public bool IsSuccess => ErrorType == ErrorType.NotError;
        public bool IsFail => !IsSuccess;

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage ?? string.Join("; ", ValidationErrors.Select(x => x.Value));
            set => _errorMessage = value;
        }

        public ErrorType ErrorType { get; protected set; }

        public Dictionary<string, List<string>> ValidationErrors { get; protected set; } = new Dictionary<string, List<string>>();

        public CallResult(ErrorType errorType)
        {
            ErrorType = errorType;
        }

        public CallResult()
        {
            ErrorType = ErrorType.NotError;
        }

        public CallResult(string errorMessage, ErrorType errorType)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public void SetResult(CallResult callResult)
        {
            ErrorType = callResult.ErrorType;
            ErrorMessage = callResult.ErrorMessage;
            ValidationErrors = callResult.ValidationErrors;
        }
    }

    public class CallResult<T> : CallResult
    {
        public CallResult(T data, ErrorType errorType = ErrorType.NotError) : base(errorType)
        {
            Data = errorType == ErrorType.NotError ? data : default;
        }

        public CallResult(CallResult callResult) : base(callResult.ErrorType)
        {
            ErrorMessage = callResult.ErrorMessage;
            ValidationErrors = callResult.ValidationErrors;
            ErrorType = callResult.ErrorType;
            ValidationErrors = callResult.ValidationErrors;
        }

        public CallResult(T data, string errorMessage, ErrorType errorType) : base(errorType)
        {
            Data = errorType == ErrorType.NotError ? data : default;
            ErrorMessage = errorMessage;
        }

        public CallResult(string errorMessage, ErrorType errorType) : base(errorType)
        {
            ErrorMessage = errorMessage;
        }

        public CallResult()
        {
        }

        public T Data { get; }
    }
}
