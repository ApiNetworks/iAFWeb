using iAF.Entities;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iAFWebHost.Api
{
    public class ApiService : Service
    {
        public object Any(UrlRequest request)
        {
            if (!String.IsNullOrEmpty(request.Url))
                return ShortenUrl(request);

            if (request.ShortId.IsShortCode())
                return ExpandUrl(request);

            return new ArgumentNullException();
        }

        public object ShortenUrl(UrlRequest request)
        {
            if (String.IsNullOrEmpty(request.Url))
                throw new ArgumentNullException(request.Url);

            Uri uri;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out uri) == false)
                throw new ArgumentNullException(request.Url);

            if (uri.AbsoluteUri.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) == false
                && uri.AbsoluteUri.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) == false
                && uri.AbsoluteUri.StartsWith("ftp", StringComparison.InvariantCultureIgnoreCase) == false)
                throw new ArgumentException(request.Url);
            
            Url entity = new Url();
            entity.Href = uri.AbsoluteUri;
            iAFWebHost.Entities.Url dbEntity = Mapper.MapResponse(entity);

            UrlService service = new UrlService();
            iAFWebHost.Entities.Url dbResponse = service.ShortenUrl(dbEntity);
            entity = Mapper.MapResponse(dbResponse);

            return entity;
        }

        public object ExpandUrl(UrlRequest request)
        {
            if (!request.ShortId.IsShortCode())
                throw new ArgumentNullException(request.ShortId);

            UrlService service = new UrlService();
            var dbResponse = service.ExpandUrl(request.ShortId);
            return Mapper.MapResponse(dbResponse);
        }
    }
}