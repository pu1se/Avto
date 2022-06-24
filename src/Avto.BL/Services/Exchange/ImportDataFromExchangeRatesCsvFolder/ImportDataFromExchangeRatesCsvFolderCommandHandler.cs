using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;

namespace Avto.BL.Services.Exchange.ImportDataFromExchangeRatesCsvFolder
{
    public class ImportDataFromExchangeRatesCsvFolderCommandHandler : CommandHandler<EmptyCommand, CallResult>
    {
        public ImportDataFromExchangeRatesCsvFolderCommandHandler(Storage storage, LogService logService) : base(storage, logService)
        {
        }

        protected override Task<CallResult> HandleCommandAsync(EmptyCommand command)
        {
            
            return Task.FromResult(SuccessResult());
        }
    }
}
