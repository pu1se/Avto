using System;
using System.Threading.Tasks;
using Avto.BL._Core;
using Avto.DAL;

namespace Avto.BL
{
    public abstract class QueryHandler<TQuery, TResult> : CallResultShortcuts, IHandler 
        where TResult : CallResult, new() 
        where TQuery : Query
    {
        protected Storage Storage { get; }
        protected LogService LogService { get; }

        protected QueryHandler(Storage storage, LogService logService)
        {
            Storage = storage;
            Storage.UseNoTracking = true;
            LogService = logService;
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

        protected async Task<CallResult<T>> SafeCallAsync<T>(Func<Task<T>> serviceMethod)
        {
            try
            {
                var result = await serviceMethod();
                return new CallResult<T>(result);
            }
            catch (ThirdPartyApiException exception)
            {
                LogService.WriteError(exception);
                return new CallResult<T>(exception.Message, ErrorType.ThirdPartyApiError417);
            }
            catch (Exception exception)
            {
                LogService.WriteError(exception);
                return new CallResult<T>(exception.Message, ErrorType.UnexpectedError500);
            }
        }
    }
}
