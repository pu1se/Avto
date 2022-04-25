using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avto.UI.Front.ApiRequests
{
    public abstract class BaseApiClient
    {
        protected ApiRequestExecuter Api { get; }
        private UiAppSettings Settings { get; }

        protected BaseApiClient(UiAppSettings settings)
        {
            Settings = settings;
            Api = new ApiRequestExecuter(Settings.PaymentApiUrl, settings.PaymentApiAuthClientId, settings.PaymentApiAuthClientSecret);
        }
    }
}
