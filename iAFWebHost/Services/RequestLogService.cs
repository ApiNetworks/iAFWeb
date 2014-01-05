using iAFWebHost.Entities;
using iAFWebHost.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace iAFWebHost.Services
{
    public class RequestLogService : BaseService
    {
        private RequestLogRepository _repository; 

        public RequestLogService()
        {
            _repository = new RequestLogRepository();
        }

        public void Log(RequestLog log)
        {
            if (log == null)
                throw new ArgumentException("Log value is invalid");

            try
            {
                _repository.Save(log, TimeSpan.FromHours(25));
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { log.Id }, ex);
            }
        }
    }
}