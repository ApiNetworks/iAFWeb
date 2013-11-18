using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Entities
{
    public class ServiceError : Exception
    {
        public string Message { get; set; }
    }
}