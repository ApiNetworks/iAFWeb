using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace iAFWebHost.Utils
{
    public class HttpResponseDTO
    {
        public int StatusCode { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string ResponseIP { get; set; }
        public Uri ResponseUri { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public static class HttpRequestHelper
    {
        public static HttpResponseDTO GetResponse(Uri url, int timeoutInSeconds)
        {
            HttpResponseDTO dto = new HttpResponseDTO();
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(url);
                request.Proxy = new WebProxy();
                request.Method = "HEAD";
                request.AllowAutoRedirect = true;
                request.Timeout = timeoutInSeconds * 1000;
                request.ServicePoint.BindIPEndPointDelegate = delegate(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
                {
                    if (remoteEndPoint != null)
                    {
                        dto.ResponseIP = remoteEndPoint.Address.ToString();
                    }
                    return null;
                };

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    dto.StatusCode = (int)response.StatusCode;
                    dto.ResponseUri = response.ResponseUri;
                    dto.ContentLength = response.ContentLength;
                    dto.ContentType = response.ContentType;
                    dto.TimeStamp = DateTime.UtcNow;
                }
            }
            catch (Exception)
            {

            }
            return dto;
        }
    }
}