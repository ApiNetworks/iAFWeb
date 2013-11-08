using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using iAFWebHost.Entities;
using iAFWebHost.Utils;

namespace iAFWebHost.Services
{
    public class UrlService : BaseService
    {
        public List<Url> Upsert(List<Url> urlList)
        {
            if (urlList.Count > 1000)
                throw new ArgumentException("Url list cannot hold more than 1000 items");

            ConcurrentBag<Url> collection = new ConcurrentBag<Url>();

            Parallel.ForEach<Url>(urlList, item =>
            {
                if (IsValid(item))
                {
                    var url = Upsert(item);
                    collection.Add(url);
                }
            });

            return collection.ToList<Url>();
        }

        public Url Upsert(Url url)
        {
            if (url == null)
                throw new ArgumentNullException("Url entity can not be null");

            if (String.IsNullOrEmpty(url.Host))
                throw new ArgumentNullException("Host property cannot be null");

            string hashKey = url.Href.MD5();
            ulong longId = NoSqlClient.Get<ulong>(hashKey);
            if (longId.Equals(0))
            {
                url.Id = Increment();
                url.Flag = 1;

                var result = NoSqlClient.ExecuteStoreJson(StoreMode.Set, url.Id.ToString(), url);

                if (result.Success)
                    NoSqlClient.Store(StoreMode.Add, hashKey, url.Id);
                else
                    url.Flag = 0;
            }
            else
            {
                Url entity = GetById(longId);

                // Upsert users into collections
                if (url.Users.Count > 0)
                {
                    var userList = entity.Users;
                    userList.AddRange(url.Users);
                    entity.Users = userList.Distinct().ToList();
                    NoSqlClient.ExecuteStoreJson(StoreMode.Replace, entity.Id.ToString(), entity);
                }

                // assign return value
                url = entity;
            }

            return url;
        }

        public Url GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Id can't be null or an empty string");

            ulong databaseId = 0;
            databaseId = id.DecodeBase58();
            return GetById(databaseId);
        }

        public Url GetById(ulong id)
        {
            return NoSqlClient.GetJson<Url>(id.ToString());
        }

        public Url AddUser(string id, string userName)
        {
            Url entity = null;
            if(id.IsShortCode())
            {
                entity = GetById(id);
                if (entity != null 
                    && entity.Users != null 
                    && entity.Users.Contains(userName) == false)
                {
                    entity.Users.Add(userName);
                    NoSqlClient.ExecuteStoreJson(StoreMode.Replace, entity.Id.ToString(), entity);
                }
            }
            return entity;
        }

        public Url DeleteUser(string id, string userName)
        {
            Url entity = null;
            if (id.IsShortCode())
            {
                entity = GetById(id);
                if (entity != null
                    && entity.Users != null
                    && entity.Users.Contains(userName) == true)
                {
                    entity.Users.Remove(userName);
                    NoSqlClient.ExecuteStoreJson(StoreMode.Replace, entity.Id.ToString(), entity);
                }
            }
            return entity;
        }

        public int GetUrlCount()
        {
            var view = NoSqlClient.GetView<int>("url", "urlcount");
            return view.FirstOrDefault();
        }

        public List<Url> GetUrlList(string startKey = null, string endKey = null, int limit = 0)
        {
            if (limit > 100)
                limit = 100;

            var view = NoSqlClient.GetView("url", "urllist");
            if (limit > 0) view.Limit(limit);
            if (!string.IsNullOrEmpty(startKey)) view.StartKey(startKey);
            if (!string.IsNullOrEmpty(endKey)) view.EndKey(endKey);
            view.Descending(true);

            var results = view;

            List<Url> urlList = new List<Url>();
            foreach (var row in results)
            {
                Url url = GetById(row.ViewKey[0].ToString());
                if (url != null)
                    urlList.Add(url);
            }

            return urlList;
        }

        public List<Url> GetUserUrlList(string userName, string startKey = null, string endKey = null, int limit = 0)
        {
            if (limit > 100)
                limit = 100;

            var view = NoSqlClient.GetView("user", "user_urllist");
            if (limit > 0) view.Limit(limit);
            if (String.IsNullOrEmpty(startKey)) view.StartKey(userName);
            if (String.IsNullOrEmpty(endKey)) view.EndKey(userName + "999999999999");
            view.Stale(Couchbase.StaleMode.False);

            var results = view;

            List<Url> urlList = new List<Url>();
            foreach (var row in results)
            {
                var item = row.Info.Values.ToList();
                Url url = GetById(item[2].ToString());
                if (url != null)
                    urlList.Add(url);
            }

            return urlList;
        }

        public int GetUserUrlCount(string userName)
        {
            var view = NoSqlClient.GetView("user", "user_urlcount");
            view.StartKey(userName);
            view.EndKey(userName);
            view.Group(true);
            view.GroupAt(1);
            var results = view;

            int urlCount = 0;
            foreach (var row in results)
            {
                var item = row.Info.Values.ToList();
                var val = item[1].ToString();
                urlCount = Int32.Parse(val);
            }

            return urlCount;
        }

        public Url ShortenUrl(Url url)
        {
            return Upsert(url);
        }

        public bool IsValid(Url url)
        {
            if (url != null && String.IsNullOrEmpty(url.Host) == false)
                return true;
            else
                return false;
        }
    }
}