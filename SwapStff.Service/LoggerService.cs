using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public class LoggerService : ILoggerService
    {

       /* private IRepository<LogError> errorRepository;
        public LoggerService(IRepository<LogError> errorRepository)
        {
            this.errorRepository = errorRepository;
        }
        public LoggerService(Exception ex)
        {
            LogError model = new LogError();
            model.Date = DateTime.Now;
            model.Thread  = ex.TargetSite.ToString();
            model.Level = ex.InnerException.ToString();
            model.Message = ex.Message;
            model.Exception = ex.ToString();
        }

         public void Insert(LogError model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Error");
            }

            errorRepository.Insert(model);
        }*/

        
        private ILog _logger;

        public LoggerService()
        {
            _logger = LogManager.GetLogger(this.GetType());
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception x)
        {
            Error(SwapStff.Core.Logging.LogUtility.BuildExceptionMessage(x));
        }

        public void Error(string message, Exception x)
        {
            _logger.Error(message, x);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(SwapStff.Core.Logging.LogUtility.BuildExceptionMessage(x));
        }
         
    }
}
