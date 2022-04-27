using System;
using System.Threading.Tasks;
using Avto.BL._Core;
using Avto.DAL;

namespace Avto.BL
{
    public abstract class CommandHandler<TCommand, TResult> : CallResultShortcuts, IHandler 
        where TResult : CallResult, new() 
        where TCommand : Command
    {
        protected Storage Storage { get; }
        protected LogService LogService { get; }

        protected CommandHandler(Storage storage, LogService logService)
        {
            Storage = storage;
            Storage.UseNoTracking = false;
            LogService = logService;
        }

        protected abstract Task<TResult> HandleCommandAsync(TCommand command);

        private TResult ValidateModel(TCommand command)
        {
            if (!command.IsValid())
            {
                var result = new TResult();
                var validationResult = Result.ValidationFail(command);
                result.SetResult(validationResult);
                return result;
            }

            return new TResult();
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            var validationResult = ValidateModel(command);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            return await HandleCommandAsync(command);
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
                LogService.WriteError(exception);
                return new CallDataResult<T>(exception.Message, ErrorType.ThirdPartyApiError417);
            }
            catch (Exception exception)
            {
                LogService.WriteError(exception);
                return new CallDataResult<T>(exception.Message, ErrorType.UnexpectedError500);
            }
        }
    }
}
