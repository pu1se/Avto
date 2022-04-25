using System.Collections.Generic;
using System.Linq;

namespace PaymentMS
{
    public class CallResult
    {
        public bool IsSuccess => ErrorType == ErrorType.NotError;

        public string ErrorMessage { get; protected set; }

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
}
