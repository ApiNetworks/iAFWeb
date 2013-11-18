using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceClient.Web;
using iAF.Entities;

namespace iAF
{
    public class HttpClient : IHttpClient
    {
        private static string endPoint = "http://i.af/v1";
        //private static string endPoint = "http://dev.i.af/v1";

        private static readonly Lazy<JsonServiceClient> _instance = new Lazy<JsonServiceClient>(() => new JsonServiceClient(endPoint));

        private static JsonServiceClient Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public List<Url> Shorten(List<string> uris)
        {
            List<Url> responses = new List<Url>();
            foreach(string uri in uris)
            {
                Uri validUri = null;
                if (!String.IsNullOrEmpty(uri) && Uri.TryCreate(uri, UriKind.Absolute, out validUri))
                {
                    Url response = Shorten(uri);
                    if (response != null)
                        responses.Add(response);
                }
            }
            return responses;
        }

        public Url Shorten(string uri)
        {
            UrlRequest request = new UrlRequest();
            Uri validUri = null;
            if (!String.IsNullOrEmpty(uri) && Uri.TryCreate(uri, UriKind.Absolute, out validUri))
            {
                request.Url = uri;
                return Instance.Get(request);
            }
            else
            {
                throw new ArgumentException("Error: Unable to parse uri parameter");
            }
        }

        public List<Url> Expand(List<string> shortIds)
        {
            List<Url> responses = new List<Url>();
            foreach (string shortId in shortIds)
            {
                if (shortId.IsShortCode())
                {
                    Url response = Expand(shortId);
                    if (response != null)
                        responses.Add(response);
                }
            }
            return responses;
        }

        public Url Expand(string shortId)
        {
            if (shortId.IsShortCode())
            {
                UrlRequest request = new UrlRequest();
                request.ShortId = shortId;
                return Instance.Get(request);
            }
            else
            {
                throw new ArgumentException("Error: Unable to parse shortId parameter");
            }
        }
    }
}
