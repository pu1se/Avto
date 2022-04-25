using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL._Core;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL
{
    public abstract class CommandHandler<TCommand, TResult> : CallResultShortcuts, IHandler 
        where TResult : CallResult, new() 
        where TCommand : Command
    {
        protected Storage Storage { get; }
        protected LogService Logger { get; }

        protected CommandHandler(Storage storage, LogService logger)
        {
            Storage = storage;
            Storage.UseNoTracking = false;
            Logger = logger;
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
