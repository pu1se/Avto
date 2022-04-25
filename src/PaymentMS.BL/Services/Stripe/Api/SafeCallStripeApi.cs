using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace PaymentMS.BL.Services.Stripe.Api
{    
    public class SafeCallStripeApi
    {
        private IStripeApi StripeApi { get; }
        private LogService Logger { get; }

        public SafeCallStripeApi(IStripeApi stripeApi, LogService logger)
        {
            StripeApi = stripeApi;
            Logger = logger;
        }

        public async Task<CallDataResult<TResult>> SafeCall<TResult>(Func<IStripeApi, Task<TResult>> serviceMethod)
        {
            try
            {
                var result = await serviceMethod(StripeApi);
                return new CallDataResult<TResult>(result);
            }
            catch (StripeException exception)
            {
                var errorMessage = exception.Message;

                Logger.WriteError(exception);
                Logger.WriteError(exception.StripeResponse?.Content);

                if (exception.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ValidationFailResult<TResult>("SecretKey", errorMessage);
                }

                if (exception.StripeError != null && !exception.StripeError.Param.IsNullOrEmpty())
                {
                    return new ValidationFailResult<TResult>(exception.StripeError.Param, errorMessage);
                }

                return new CallDataResult<TResult>(errorMessage, ErrorType.ThirdPartyApiError417);
            }
            catch (ThirdPartyApiException exception)
            {
                Logger.WriteError(exception);
                return new CallDataResult<TResult>(exception.Message, ErrorType.ThirdPartyApiError417);
            }
            catch (Exception exception)
            {
                Logger.WriteError(exception);
                return new CallDataResult<TResult>(exception.Message, ErrorType.UnexpectedError500);
            }
        }
    }
}