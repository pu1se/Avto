using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL._Core.Logger;
using Avto.DAL.Entities;
using Avto.DAL.Repositories;

namespace Avto.BL
{
    public class LogService
    {
        private List<LogItem> LogList { get; } = new List<LogItem>();
        private Storage Storage { get; }
        public Guid LogId { get; private set; }
        private LogHttpInfo httpInfo { get; set; } = new LogHttpInfo();

        public LogService(Storage storage)
        {
            Storage = storage;
        }

        public void Init()
        {
            if (LogId == Guid.Empty)
            {
                LogId = Guid.NewGuid();
            }
        }

        public bool IsInit()
        {
            return LogId != Guid.Empty;
        }

        public void AddHttpInfo(LogHttpInfo requestInfo)
        {
            httpInfo = requestInfo;
        }

        public void WriteInfo(string message)
        {
            Init();
            LogList.Add(new LogItem(message, LogType.Info));
        }

        public void WriteError(string message)
        {
            Init();
            LogList.Add(new LogItem(message, LogType.Error));
        }

        public void WriteError(Exception exception)
        {
            Init();
            LogList.Add(new LogItem(exception.ToFormattedString(), LogType.Error));
        }

        public bool HasErrors => LogList.Any(x => x.Type == LogType.Error);

        public Task SaveAsync()
        {
            return Storage.ApiLogs.AddAsync(new ApiLogEntity
            {
                Id = LogId,
                Logs = LogList.ToJson().TrimByLength(8192),
                PathToAction = httpInfo.PathToAction.TrimByLength(1024),
                HttpMethod = httpInfo.HttpMethod.TrimByLength(32),
                ResponseCode = httpInfo.ResponseCode,
                ExecutionTimeInMilliSec = httpInfo.ExecutionTimeInMilliSec
            });
        }
    }
}
