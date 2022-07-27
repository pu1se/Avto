using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;

namespace Avto.BL.Services.System.SendSystemWarningEmail
{
    public class SendSystemWarningEmailCommandHandler : CommandHandler<SendSystemWarningEmailCommand, CallResult>
    {
        private AppSettings Settings { get; }

        public SendSystemWarningEmailCommandHandler(AppSettings settings, Storage storage, LogService logService) : base(storage, logService)
        {
            Settings = settings;
        }

        protected override async Task<CallResult> HandleCommandAsync(SendSystemWarningEmailCommand command)
        {
            await SendEmail.ToMyself(
                $"Avto {Settings.Environment} System warning!", 
                command.Message
            );

            return SuccessResult();
        }
    }
}
