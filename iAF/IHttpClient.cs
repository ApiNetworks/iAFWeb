using iAF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAF
{
    public interface IHttpClient
    {
        Url Shorten(string uri);
        List<Url> Shorten(List<string> uris);

        Url Expand(string shortId);
        List<Url> Expand(List<string> shortIds);
    }
}
