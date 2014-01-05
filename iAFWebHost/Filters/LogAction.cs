using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using iAFWebHost.Utils;

namespace iAFWebHost.Filters
{
    public class LogHttpRequestAction : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    
                }
                catch
                {

                }
            });
        }
    }
}