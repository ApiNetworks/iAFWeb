using iAFWebHost.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace iAFWebHost.Services
{
    public static class UrlServiceHelper
    {
        public static void ResolveUrl(Url url)
        {
            Task.Factory.StartNew((u) =>
                {
                    try
                    {
                        UrlService urlService = new UrlService();
                        if (url != null)
                        {
                            urlService.ResolveResponseUrl(url);
                        }
                    }
                    catch
                    {

                    }
                },
                url);
        }
    }
}