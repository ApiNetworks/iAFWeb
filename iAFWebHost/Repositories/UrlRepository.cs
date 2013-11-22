using Couchbase;
using Enyim.Caching.Memcached;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace iAFWebHost.Repositories
{
    public partial class UrlRepository : RepositoryBase<Url>
    {
        #region Url
        public Url Upsert(Url url)
        {
            string hashKey = url.Href.MD5();
            string longId = GetValue(hashKey);
            if (String.IsNullOrEmpty(longId))
            {
                url.Id = base.Increment("url::count").ToString();
                url.Flag = 1;

                ulong CasResult = Save(url);
                if (CasResult > 0)
                    Save(hashKey, url.Id);
                else
                    url.Flag = 0;
            }
            else
            {
                // Upsert user collections
                if (!url.Users.IsNullOrEmpty())
                    url = AddUser(longId, url.Users);
                else
                    url = Get(longId);
            }

            return url;
        }

        public override bool Remove(string id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                string hashKey = entity.Href.MD5();
                if (base.Remove(id))
                {
                    return base.Remove(hashKey);
                }
            }
            return false;
        }

        public int GetUrlCount()
        {
            int count = 0;
            var view = View("url_list");
            var row = view.FirstOrDefault();
            var result = row.Info["value"];
            if (result != null)
            {
                string val = result.ToString();
                if (!String.IsNullOrEmpty(val))
                    Int32.TryParse(val, out count);
            }

            return count;
        }

        public Dto<Url> GetUrlList(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            return GetDto("url_list", StaleMode.AllowStale, page, limit, skip, false, 0, false, startKey, endKey, startDocId, endDocId, sort);
        }

        #endregion

        #region User
        public int GetUrlCountByUser(string userName)
        {
            var view = View("user_urllist");
            if (!String.IsNullOrEmpty(userName))
            {
                var key = FormatStringKey(userName);
                view.StartKey(key);
                view.EndKey(key);
            }
            view.Reduce(true);
            int count = 0;
            var row = view.FirstOrDefault();
            if (row != null)
            {
                var result = row.Info["value"];
                if (result != null)
                {
                    string val = result.ToString();
                    if (!String.IsNullOrEmpty(val))
                        Int32.TryParse(val, out count);
                }
            }
            return count;
        }

        public Dto<Url> GetUrlListByUser(string userName)
        {
            return GetUrlListByHost(1, 10, 0, userName, userName, null, null, null);
        }

        public Dto<Url> GetUrlListByUser(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            string key = null;
            if (!String.IsNullOrEmpty(startKey))
                key = BuildKey(new object[] { startKey });

            return GetDto("user_urllist", StaleMode.False, page, limit, skip, false, 0, false, key, key, startDocId, endDocId, sort);
        }

        public Url AddUser(string id, List<string> userNames)
        {
            Url entity = null;
            if (id.IsShortCode())
            {
                string docId = id.DecodeBase58().ToString();
                entity = Get(docId);
                if (entity != null && !userNames.IsNullOrEmpty())
                {
                    List<string> users = entity.Users;
                    List<string> mergedUsers = users.Union(userNames).ToList();

                    // test for a change in the list count
                    if (users.Count != mergedUsers.Count)
                    {
                        // change detected, update entity
                        entity.Users = mergedUsers;
                        Update(entity);

                        // retrieve from the view to referesh data on screen
                        GetUrlListByUser(mergedUsers.FirstOrDefault());
                    }
                }
            }
            return entity;
        }

        public Url DeleteUser(string id, List<string> userNames)
        {
            Url entity = null;
            if (id.IsShortCode())
            {
                string docId = id.DecodeBase58().ToString();
                entity = Get(docId);
                if (entity != null && !userNames.IsNullOrEmpty())
                {
                    List<string> users = entity.Users;
                    List<string> mergedUsers = users.Except(userNames).ToList();

                    // test for a change in the list count
                    if (users.Count != mergedUsers.Count)
                    {
                        // change detected, update entity
                        entity.Users = mergedUsers;
                        Update(entity);

                        // retrieve from the view to referesh data on screen
                        GetUrlListByUser(mergedUsers.FirstOrDefault());
                    }
                }
            }
            return entity;
        }
        #endregion

        #region Tags
        public int GetUrlCountByTag(string tagName)
        {
            var view = View("tag_urllist");
            if (!String.IsNullOrEmpty(tagName))
            {
                var key = FormatStringKey(tagName);
                view.StartKey(key);
                view.EndKey(key);
            }
            view.Reduce(true);
            int count = 0;
            var row = view.FirstOrDefault();
            if (row != null)
            {
                var result = row.Info["value"];
                if (result != null)
                {
                    string val = result.ToString();
                    if (!String.IsNullOrEmpty(val))
                        Int32.TryParse(val, out count);
                }
            }
            return count;
        }

        public Url AddTag(string id, List<string> tagNames)
        {
            Url entity = null;
            if (id.IsShortCode())
            {
                string docId = id.DecodeBase58().ToString();
                entity = Get(docId);
                if (entity != null && !tagNames.IsNullOrEmpty())
                {
                    List<string> tags = entity.Users;
                    List<string> mergedTags = tags.Union(tagNames).ToList();

                    // test for a change in the list count
                    if (tags.Count != mergedTags.Count)
                    {
                        // change detected, update entity
                        entity.Tags = mergedTags;
                        Update(entity);
                    }
                }
            }
            return entity;
        }

        public Url DeleteTag(string id, List<string> tagNames)
        {
            Url entity = null;
            if (id.IsShortCode())
            {
                string docId = id.DecodeBase58().ToString();
                entity = Get(docId);
                if (entity != null && !tagNames.IsNullOrEmpty())
                {
                    List<string> tags = entity.Users;
                    List<string> mergedUsers = tags.Except(tagNames).ToList();

                    // test for a change in the list count
                    if (tags.Count != mergedUsers.Count)
                    {
                        // change detected, update entity
                        entity.Users = mergedUsers;
                        Update(entity);
                    }
                }
            }
            return entity;
        }

        public Dto<Url> GetUrlListByTag(string tag)
        {
            return GetUrlListByTag(1, 10, 0, tag, tag, null, null, null);
        }

        public Dto<Url> GetUrlListByTag(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            string key = null;
            if (!String.IsNullOrEmpty(startKey))
                key = BuildKey(new object[] { startKey });

            return GetDto("tag_urllist", StaleMode.AllowStale, page, limit, skip, false, 0, false, key, key, sort);
        }
        #endregion

        #region Host
        public int GetUrlCountByHost(string host)
        {
            var view = View("host_urllist");
            if (!string.IsNullOrEmpty(host))
            {
                var key = FormatStringKey(host);
                view.StartKey(key);
                view.EndKey(key);
            }
            view.Reduce(true);

            int count = 0;
            var row = view.FirstOrDefault();
            if (row != null)
            {
                var result = row.Info["value"];
                if (result != null)
                {
                    string val = result.ToString();
                    if (!String.IsNullOrEmpty(val))
                        Int32.TryParse(val, out count);
                }
            }

            return count;
        }

        public Dto<Url> GetUrlListByHost(string host)
        {
            return GetUrlListByHost(1, 10, 0, host, host, null, null, null);
        }

        public Dto<Url> GetUrlListByHost(int page = 0, int limit = 10, int skip = 0, string startKey = null, string endKey = null, string startDocId = null, string endDocId = null, string sort = null)
        {
            string key = null;
            if (!String.IsNullOrEmpty(startKey))
                key = BuildKey(new object[] { startKey });

            return GetDto("host_urllist", StaleMode.AllowStale, page, limit, skip, false, 0, false, key, key, startDocId, endDocId, sort);
        }
        #endregion

        #region Statistics
        public ulong IncrementHitCount(DataPoint entity)
        {
            return base.Increment(entity.BuildKey());
        }

        public DataPoint GetDataPointValue(DataPoint entity)
        {
            string dbKey = entity.BuildKey();

            DataPoint result = new DataPoint();
            result.Id = dbKey;
            result.Value = 0;
            result.UtcTimeStamp = entity.UtcTimeStamp;

            string value = base.GetValue(dbKey);
            ulong countValue = 0;
            if (!String.IsNullOrEmpty(value) && ulong.TryParse(value, out countValue))
            {
                result.Id = dbKey;
                result.ParseKey();
                result.Value = countValue;
            }

            return result;
        }

        public List<DataPoint> GetHourlySystemStats(DateTime startDate, DateTime endDate)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            object[] startKey = { startDate.Year.ToString(), startDate.Month.ToString(), startDate.Day.ToString(), startDate.Hour.ToString() };
            object[] endKey = { endDate.Year.ToString(), endDate.Month.ToString(), endDate.Day.ToString(), endDate.Hour.ToString() };

            dataPoints = GetSystemStatsAggregate(startKey, endKey, 48, true, 4, true);

            return dataPoints;
        }

        public List<DataPoint> GetSystemStatsAggregate(object[] startKey, object[] endKey, int limit, bool group, int groupLevel, bool reduce)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            if (limit > 1000)
                limit = 1000;

            var view = View("stats", "all");
            if (startKey != null)
                view.StartKey(startKey);
            if (startKey != null)
                view.EndKey(endKey);
            if (group) view.Group(true);
            if (groupLevel > 0) view.GroupAt(groupLevel);
            view.Reduce(reduce);

            // retrieve results
            List<IViewRow> results = view.ToList();
            if (!results.IsNullOrEmpty())
            {
                foreach (var row in results)
                {
                    if (row.Info != null)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.ShortId = String.Empty;
                        List<object> list = row.Info.Values.ToList<object>();
                        if (!list.IsNullOrEmpty() && list.Count == 2)
                        {
                            dataPoint.UtcTimeStamp = DateTime.MinValue;
                            object[] dateTimeArray = list[0] as object[];
                            if (dateTimeArray != null && dateTimeArray.Length == 4)
                            {
                                int year = 1;
                                int month = 1;
                                int day = 1;
                                int hour = 1;
                                if (Int32.TryParse(dateTimeArray[0].ToString(), out year)
                                    && Int32.TryParse(dateTimeArray[1].ToString(), out month)
                                    && Int32.TryParse(dateTimeArray[2].ToString(), out day)
                                    && Int32.TryParse(dateTimeArray[3].ToString(), out hour))
                                {
                                    dataPoint.UtcTimeStamp = new DateTime(year, month, day, hour, 0, 0);
                                }
                            }

                            Dictionary<string, object> data = list[1] as Dictionary<string, object>;
                            if (data != null)
                            {
                                if (data.ContainsKey("sum"))
                                    dataPoint.Sum = (long)data["sum"];
                                if (data.ContainsKey("min"))
                                    dataPoint.Min = (long)data["min"];
                                if (data.ContainsKey("max"))
                                    dataPoint.Max = (long)data["max"];
                                if (data.ContainsKey("count"))
                                    dataPoint.Count = (long)data["count"];
                                if (data.ContainsKey("sumsqr"))
                                    dataPoint.SumSqr = (long)data["sumsqr"];

                                dataPoints.Add(dataPoint);
                            }
                        }
                    }
                }
            }

            return dataPoints;
        }

        #endregion
    }
}