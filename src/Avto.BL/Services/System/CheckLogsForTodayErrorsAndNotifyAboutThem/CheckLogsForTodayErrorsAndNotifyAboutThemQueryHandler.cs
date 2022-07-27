using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.System.CheckLogsForTodayErrorsAndNotifyAboutThem
{
    public class CheckLogsForTodayErrorsAndNotifyAboutThemQueryHandler : QueryHandler<EmptyQuery, CallResult>
    {
        private AppSettings Settings { get; }
        private SystemService SystemService { get; }

        public CheckLogsForTodayErrorsAndNotifyAboutThemQueryHandler(AppSettings settings, SystemService systemService, Storage storage, LogService logService) : base(storage, logService)
        {
            SystemService = systemService;
            Settings = settings;
        }

        protected override async Task<CallResult> HandleCommandAsync(EmptyQuery command)
        {
            if (Settings.Environment == "Development")
            {
                return SuccessResult();
            }

            var errors = await this.Storage.Logs
                .Where(
                    e =>
                        e.CreatedDateUtc >= DateTime.UtcNow.Date.AddDays(-1)
                        &&
                        e.ResponseCode != 200)
                .ToListAsync();

            if (errors.Any())
            {
                var mailMessage = $"Error list: {Environment.NewLine}{errors.Select(x => $"Code: {x.ResponseCode}; Path: {x.PathToAction}; Logs: {x.Logs}").Join(Environment.NewLine)}";
                await SystemService.SendSystemWarningEmail(mailMessage);
            }

            return SuccessResult();
        }
    }
}
