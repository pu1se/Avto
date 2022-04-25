using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL._Core.Logger
{
    public enum LogType
    {
        Info,
        Error
    }

    public class LogItem
    {
        public LogItem() {}

        public LogItem(string message, LogType type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; }
        public LogType Type { get; }
    }
}
