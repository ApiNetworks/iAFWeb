using iAFWebHost.Entities;
using iAFWebHost.Repositories;
using iAFWebHost.Utils;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Services
{
    public partial class LogService : BaseService, ILogger
    {
        private static Logger _logger;
        private static LogRepository _repository;

        public LogService()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _repository = new LogRepository();
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

        public void Error(string message, Exception x)
        {
            _logger.ErrorException(message, x);
        }

        public void Error(Exception x)
        {
            Error(x.BuildExceptionMessage());
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(x.BuildExceptionMessage());
        }

        public Dto<Error> GetErrors()
        {
            try
            {
                return _repository.GetErrors(1, 10, 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool DeleteError(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            if (!id.ToLower().StartsWith("error_"))
                throw new ArgumentException();

            return _repository.Remove(id);
        }
    }
}