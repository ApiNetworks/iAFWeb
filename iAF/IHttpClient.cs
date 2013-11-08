using iAF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAF
{
    public interface IHttpClient
    {
        UrlResponse Shorten(string uri);
        List<UrlResponse> Shorten(List<string> uris);

        UrlResponse Expand(string shortId);
        List<UrlResponse> Expand(List<string> shortIds);
    }
}
