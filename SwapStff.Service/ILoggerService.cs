using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    /// <summary>
    /// An interface that any logger can use
    /// </summary>
    public interface ILoggerService
    {
        void Info(string message);

        void Warn(string message);

        void Debug(string message);

        void Error(string message);
        void Error(string message, Exception x);
        void Error(Exception x);

        void Fatal(string message);
        void Fatal(Exception x);

        //void Insert(LogError model);

    }
}
