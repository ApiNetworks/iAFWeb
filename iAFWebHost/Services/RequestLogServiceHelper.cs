using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace iAFWebHost.Services
{
    public static class RequestLogServiceHelper
    {
        public static void LogAsync(RequestLog log)
        {
            Task.Factory.StartNew((u) =>
            {
                try
                {
                    RequestLogService logService = new RequestLogService();
                    if (log != null)
                    {
                        logService.Log(log);
                    }
                }
                catch
                {

                }
            },
                log);
        }
    }
}