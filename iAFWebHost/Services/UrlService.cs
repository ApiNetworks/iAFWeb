using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using iAFWebHost.Repositories;
using Newtonsoft.Json;

namespace iAFWebHost.Services
{
    public class UrlService : BaseService
    {
        private UrlRepository _repository;

        public UrlService()
        {
            _repository = new UrlRepository();
        }

        #region Url

        /// <summary>
        /// Upserts the specified URL list.
        /// </summary>
        /// <param name="urlList">The URL list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Url list cannot be null</exception>
        /// <exception cref="System.ArgumentException">Url list cannot hold more than 1000 items</exception>
        public List<Url> Upsert(List<Url> urlList)
        {
            if (urlList.IsNullOrEmpty())
                throw new ArgumentNullException("Url list cannot be null");

            if (urlList.Count > 1000)
                throw new ArgumentException("Url list cannot hold more than 1000 items");

            try
            {
                ConcurrentBag<Url> collection = new ConcurrentBag<Url>();

                Parallel.ForEach<Url>(urlList, item =>
                {
                    if (IsValid(item))
                    {
                        var url = ShortenUrl(item);
                        collection.Add(url);
                    }
                });

                return collection.ToList<Url>();
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { urlList }, ex);
            }
        }

        /// <summary>
        /// Expands the URL.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Id value is invalid</exception>
        public Url ExpandUrl(string id)
        {
            if (!id.IsShortCode())
                throw new ArgumentException("Id value is invalid");

            try
            {
                return _repository.Get(id.DecodeBase58().ToString());
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id }, ex);
            }
        }

        /// <summary>
        /// Shortens the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Url ShortenUrl(Url url)
        {
            if (url == null)
                throw new ArgumentNullException();

            try
            {
                var entity = _repository.Upsert(url);
                UrlServiceHelper.ResolveUrl(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { url }, ex);
            }
        }

        public void ResolveResponseUrl(Url url)
        {
            try
            {
                if(url.Href.IsValidUri())
                {
                    Uri uri = url.Href.TryCreateUri();
                    HttpResponseDTO dto = HttpRequestHelper.GetResponse(uri,10);
                    url.HrefActual = dto.ResponseUri.ToString();
                    url.HttpContentLength = dto.ContentLength;
                    url.HttpContentType = dto.ContentType;
                    url.HttpResponseCode = dto.StatusCode;
                    url.HttpTimeStamp = dto.TimeStamp;
                    url.HttpResponseIP = dto.ResponseIP;
                }

                _repository.Update(url);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { url }, ex);
            }
        }

        /// <summary>
        /// Gets the URL count.
        /// </summary>
        /// <returns></returns>
        public int GetUrlCount()
        {
            try
            {
                return _repository.GetUrlCount();
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }
        }

        /// <summary>
        /// Gets the URL list.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="startKey">The start key.</param>
        /// <param name="endKey">The end key.</param>
        /// <param name="startDocumentId">The start document identifier.</param>
        /// <param name="endDocumentId">The end document identifier.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public Dto<Url> GetUrlList(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocumentId = null, string endDocumentId = null, string sort = null)
        {
            try
            {
                return _repository.GetUrlList(page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort }, ex);
            }
        }

        #endregion

        #region User

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Id value is invalid
        /// or
        /// UserName value is invalid
        /// </exception>
        public Url AddUser(string id, string userName)
        {
            if (!id.IsShortCode())
                throw new ArgumentException("Id value is invalid");

            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("UserName value is invalid");

            try
            {
                List<string> users = new List<string>();
                users.Add(userName);
                return _repository.AddUser(id, users);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id, userName }, ex);
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Id value is invalid
        /// or
        /// UserName value is invalid
        /// </exception>
        public Url DeleteUser(string id, string userName)
        {
            if (!id.IsShortCode())
                throw new ArgumentException("Id value is invalid");

            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("UserName value is invalid");

            try
            {
                List<string> users = new List<string>();
                users.Add(userName);
                return _repository.DeleteUser(id, users);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { id, userName }, ex);
            }
        }

        /// <summary>
        /// Gets the URL count by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public int GetUrlCountByUser(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException(userName);

            try
            {
                return _repository.GetUrlCountByUser(userName);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { userName }, ex);
            }
        }

        /// <summary>
        /// Gets the URL list by user.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="startKey">The start key.</param>
        /// <param name="endKey">The end key.</param>
        /// <param name="startDocumentId">The start document identifier.</param>
        /// <param name="endDocumentId">The end document identifier.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Dto<Url> GetUrlListByUser(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocumentId = null, string endDocumentId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(startKey))
                throw new ArgumentNullException(startKey);

            try
            {
                return _repository.GetUrlListByUser(page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort }, ex);
            }
        }

        #endregion

        #region Tag

        /// <summary>
        /// Gets the URL count by tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public int GetUrlCountByTag(string tagName)
        {
            if (String.IsNullOrEmpty(tagName))
                throw new ArgumentNullException(tagName);

            try
            {
                return _repository.GetUrlCountByTag(tagName);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }
        }

        /// <summary>
        /// Gets the URL list by tag.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="startKey">The start key.</param>
        /// <param name="endKey">The end key.</param>
        /// <param name="startDocumentId">The start document identifier.</param>
        /// <param name="endDocumentId">The end document identifier.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Dto<Url> GetUrlListByTag(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocumentId = null, string endDocumentId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(startKey))
                throw new ArgumentNullException(startKey);

            try
            {
                return _repository.GetUrlListByTag(page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort }, ex);
            }
        }

        #endregion

        #region Host

        /// <summary>
        /// Gets the URL count by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public int GetUrlCountByHost(string host)
        {
            if (String.IsNullOrEmpty(host))
                throw new ArgumentNullException(host);

            try
            {
                return _repository.GetUrlCountByHost(host);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }
        }

        /// <summary>
        /// Gets the URL list by host.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="startKey">The start key.</param>
        /// <param name="endKey">The end key.</param>
        /// <param name="startDocumentId">The start document identifier.</param>
        /// <param name="endDocumentId">The end document identifier.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Dto<Url> GetUrlListByHost(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocumentId = null, string endDocumentId = null, string sort = null)
        {
            if (String.IsNullOrEmpty(startKey))
                throw new ArgumentNullException(startKey);

            try
            {
                return _repository.GetUrlListByHost(page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { page, limit, skip, startKey, endKey, startDocumentId, endDocumentId, sort }, ex);
            }
        }

        #endregion

        #region Statistics
        public ulong IncrementHitCount(string shortId)
        {
            if (!shortId.IsShortCode())
                throw new ArgumentException("Short Id is invalid");

            try
            {
                DataPoint dataPoint = new DataPoint(shortId);
                return _repository.IncrementHitCount(dataPoint);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { shortId }, ex);
            }
        }

        public List<DataPoint> GetLast24HourStats(string shortId)
        {
            if (!shortId.IsShortCode())
                throw new ArgumentException("Short Id is invalid");

            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddHours(-23);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime hourlyInterval = startUtcDateTime;

                do
                {
                    DataPoint request = new DataPoint();
                    request.ShortId = shortId;
                    request.UtcTimeStamp = new DateTime(hourlyInterval.Year, hourlyInterval.Month, hourlyInterval.Day, hourlyInterval.Hour, 0, 0);
                    var response = _repository.GetDataPointValue(request);
                    points.Add(response);
                    hourlyInterval = hourlyInterval.AddHours(1);
                }
                while (hourlyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { shortId }, ex);
            }

            return points;
        }

        public List<DataPoint> GetLast30DaysStats(string shortId)
        {
            if (!shortId.IsShortCode())
                throw new ArgumentException("Short Id is invalid");

            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddDays(-30);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime dailyInterval = startUtcDateTime;

                // Retrieve list of available datapoints. Not every day will contain data. 
                List<DataPoint> availableDataPoints = GetDailyStats(shortId, startUtcDateTime, endUtcDateTime);
                do
                {
                    DataPoint point = new DataPoint();
                    point.ShortId = String.Empty;
                    point.UtcTimeStamp = new DateTime(dailyInterval.Year, dailyInterval.Month, dailyInterval.Day, 0, 0, 0);

                    DataPoint selectedDataPoint = (from p in availableDataPoints where p.UtcTimeStamp.Equals(point.UtcTimeStamp) select p).FirstOrDefault();
                    if (selectedDataPoint != null)
                        points.Add(selectedDataPoint);
                    else
                        points.Add(point);

                    dailyInterval = dailyInterval.AddDays(1);
                }
                while (dailyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }

            return points;
        }

        public List<DataPoint> GetLast12MonthStats(string shortId)
        {
            if (!shortId.IsShortCode())
                throw new ArgumentException("Short Id is invalid");

            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddMonths(-12);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime monthlyInterval = startUtcDateTime;

                // Retrieve list of available datapoints. Not every day will contain data. 
                List<DataPoint> availableDataPoints = GetMonthlyStats(shortId, startUtcDateTime, endUtcDateTime);
                do
                {
                    DataPoint point = new DataPoint();
                    point.ShortId = String.Empty;
                    point.UtcTimeStamp = new DateTime(monthlyInterval.Year, monthlyInterval.Month, 1, 0, 0, 0);

                    DataPoint selectedDataPoint = (from p in availableDataPoints where p.UtcTimeStamp.Equals(point.UtcTimeStamp) select p).FirstOrDefault();
                    if (selectedDataPoint != null)
                        points.Add(selectedDataPoint);
                    else
                        points.Add(point);

                    monthlyInterval = monthlyInterval.AddMonths(1);
                }
                while (monthlyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }

            return points;
        }

        public List<DataPoint> GetDailyStats(string id, DateTime startUtcDateTime, DateTime endUtcDateTime)
        {
            try
            {
                return _repository.GetDailyStats(id, startUtcDateTime, endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { startUtcDateTime, endUtcDateTime }, ex);
            }
        }

        public List<DataPoint> GetMonthlyStats(string id, DateTime startUtcDateTime, DateTime endUtcDateTime)
        {
            try
            {
                return _repository.GetMonthlyStats(id, startUtcDateTime, endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { startUtcDateTime, endUtcDateTime }, ex);
            }
        }

        public List<DataPoint> GetLast24HourSystemStats()
        {
            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddHours(-23);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime hourlyInterval = startUtcDateTime;

                // Retrieve list of available datapoints. Not every hour will contain data. 
                List<DataPoint> availableDataPoints = GetHourlySystemStats(startUtcDateTime, endUtcDateTime);
                do
                {
                    DataPoint point = new DataPoint();
                    point.ShortId = String.Empty;
                    point.UtcTimeStamp = new DateTime(hourlyInterval.Year, hourlyInterval.Month, hourlyInterval.Day, hourlyInterval.Hour, 0, 0);

                    DataPoint selectedDataPoint = (from p in availableDataPoints where p.UtcTimeStamp.Equals(point.UtcTimeStamp) select p).FirstOrDefault();
                    if (selectedDataPoint != null)
                        points.Add(selectedDataPoint);
                    else
                        points.Add(point);

                    hourlyInterval = hourlyInterval.AddHours(1);
                }
                while (hourlyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }

            return points;
        }

        public List<DataPoint> GetLast30DaysSystemStats()
        {
            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddDays(-30);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime dailyInterval = startUtcDateTime;

                // Retrieve list of available datapoints. Not every day will contain data. 
                List<DataPoint> availableDataPoints = GetDailySystemStats(startUtcDateTime, endUtcDateTime);
                do
                {
                    DataPoint point = new DataPoint();
                    point.ShortId = String.Empty;
                    point.UtcTimeStamp = new DateTime(dailyInterval.Year, dailyInterval.Month, dailyInterval.Day, 0, 0, 0);

                    DataPoint selectedDataPoint = (from p in availableDataPoints where p.UtcTimeStamp.Equals(point.UtcTimeStamp) select p).FirstOrDefault();
                    if (selectedDataPoint != null)
                        points.Add(selectedDataPoint);
                    else
                        points.Add(point);

                    dailyInterval = dailyInterval.AddDays(1);
                }
                while (dailyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }

            return points;
        }

        public List<DataPoint> GetLast12MonthSystemStats()
        {
            List<DataPoint> points = new List<DataPoint>();
            try
            {
                DateTime startUtcDateTime = DateTime.UtcNow.AddMonths(-12);
                DateTime endUtcDateTime = DateTime.UtcNow;
                DateTime monthlyInterval = startUtcDateTime;

                // Retrieve list of available datapoints. Not every day will contain data. 
                List<DataPoint> availableDataPoints = GetMonthlySystemStats(startUtcDateTime, endUtcDateTime);
                do
                {
                    DataPoint point = new DataPoint();
                    point.ShortId = String.Empty;
                    point.UtcTimeStamp = new DateTime(monthlyInterval.Year, monthlyInterval.Month, 1, 0, 0, 0);

                    DataPoint selectedDataPoint = (from p in availableDataPoints where p.UtcTimeStamp.Equals(point.UtcTimeStamp) select p).FirstOrDefault();
                    if (selectedDataPoint != null)
                        points.Add(selectedDataPoint);
                    else
                        points.Add(point);

                    monthlyInterval = monthlyInterval.AddMonths(1);
                }
                while (monthlyInterval <= endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(null, ex);
            }

            return points;
        }

        public List<DataPoint> GetHourlySystemStats(DateTime startUtcDateTime, DateTime endUtcDateTime)
        {
            try
            {
                return _repository.GetHourlySystemStats(startUtcDateTime, endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { startUtcDateTime, endUtcDateTime }, ex);
            }
        }

        public List<DataPoint> GetDailySystemStats(DateTime startUtcDateTime, DateTime endUtcDateTime)
        {
            try
            {
                return _repository.GetDailySystemStats(startUtcDateTime, endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { startUtcDateTime, endUtcDateTime }, ex);
            }
        }

        public List<DataPoint> GetMonthlySystemStats(DateTime startUtcDateTime, DateTime endUtcDateTime)
        {
            try
            {
                return _repository.GetMonthlySystemStats(startUtcDateTime, endUtcDateTime);
            }
            catch (Exception ex)
            {
                throw HandleException(new object[] { startUtcDateTime, endUtcDateTime }, ex);
            }
        }
        #endregion

        public bool IsValid(Url url)
        {
            if (url != null && String.IsNullOrEmpty(url.Host) == false)
                return true;
            else
                return false;
        }
    }
}