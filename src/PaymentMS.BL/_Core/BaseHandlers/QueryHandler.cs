using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL._Core;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL
{
    public abstract class QueryHandler<TQuery, TResult> : CallResultShortcuts, IHandler 
        where TResult : CallResult, new() 
        where TQuery : Query
    {
        protected Storage Storage { get; }
        protected LogService Logger { get; }

        protected QueryHandler(Storage storage, LogService logger)
        {
            Storage = storage;
            Storage.UseNoTracking = true;
            Logger = logger;
        }

        protected abstract Task<TResult> HandleCommandAsync(TQuery query);

        private TResult ValidateModel(TQuery query)
        {
            if (!query.IsValid())
            {
                var result = new TResult();
                var validationResult = Result.ValidationFail(query);
                result.SetResult(validationResult);
                return result;
            }

            return new TResult();
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            var validationResult = ValidateModel(query);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            return await HandleCommandAsync(query);
        }

        protected async Task<CallDataResult<T>> SafeCallAsync<T>(Func<Task<T>> serviceMethod)
        {
            try
            {
                var result = await serviceMethod();
                return new CallDataResult<T>(result);
            }
            catch (ThirdPartyApiException exception)
            {
                Logger.WriteError(exception);
                return new CallDataResult<T>(exception.Message, ErrorType.ThirdPartyApiError417);
            }
            catch (Exception exception)
            {
                Logger.WriteError(exception);
                return new CallDataResult<T>(exception.Message, ErrorType.UnexpectedError500);
            }
        }
    }
}
