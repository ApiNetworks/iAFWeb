using Couchbase;
using iAFWebHost.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Services
{
    /// <summary>
    /// Base service class for all CouchBase operations and service calls
    /// </summary>
    public class BaseService
    {
        public ServiceError HandleException(object[] param, Exception ex)
        {
            ServiceError error = new ServiceError();
            error.Message = "Service Error. Administrator has been notified";

            try
            {
                ILogger logService = new LogService();
                logService.Error(TrySerialize(param), ex);
            }
            catch
            {

            }

            return error;
        }

        private string TrySerialize(object[] param)
        {
            string returnValue = "Unable to serialize.";

            if (param == null)
                return "Null value";

            try
            {
                return JsonConvert.SerializeObject(param);
            }
            catch
            {

            }

            return returnValue;
        }
    }
}