namespace PaymentMS
{
    public class CallDataResult<T> : CallResult
    {
        public CallDataResult(T data, ErrorType errorType = ErrorType.NotError) : base(errorType)
        {
            Data = errorType == ErrorType.NotError ? data : default;
        }

        public CallDataResult(CallResult callResult) : base(callResult.ErrorType)
        {
            ErrorMessage = callResult.ErrorMessage;
            ValidationErrors = callResult.ValidationErrors;
        }

        public CallDataResult(string errorMessage, ErrorType errorType) : base(errorType)
        {
            ErrorMessage = errorMessage;
        }

        public CallDataResult()
        {
        }

        public T Data { get; }
    }
}
