using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using iAFWebHost.Entities;
using iAFWebHost.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;

namespace iAFWebHost.Repositories
{
    public abstract class RepositoryBase<T> where T : Entities.EntityBase
    {
        public virtual ulong Create(T entity)
        {
            entity.CasValue = store(StoreMode.Add, entity);
            return entity.CasValue;
        }

        public virtual ulong Update(T entity)
        {
            entity.CasValue = store(StoreMode.Replace, entity);
            return entity.CasValue;
        }

        public virtual ulong Save(T entity)
        {
            entity.CasValue = store(StoreMode.Set, entity);
            return entity.CasValue;
        }

        public virtual ulong Save(T entity, TimeSpan validFor)
        {
            entity.CasValue = store(StoreMode.Set, entity, validFor);
            return entity.CasValue;
        }

        public virtual ulong Save(string id, string entity)
        {
            return store(StoreMode.Set, id, entity);
        }

        private ulong store(StoreMode mode, T entity)
        {
            var result = CouchbaseManager.Instance.ExecuteCasJson(mode, entity.Id, entity, entity.CasValue);
            return result.Success ? result.Cas : 0;
        }

        private ulong store(StoreMode mode, T entity, TimeSpan validFor)
        {
            var result = CouchbaseManager.Instance.ExecuteCasJson(mode, entity.Id, entity, validFor, entity.CasValue);
            return result.Success ? result.Cas : 0;
        }

        private ulong store(StoreMode mode, string id, string entity)
        {
            var result = CouchbaseManager.Instance.ExecuteCas(mode, id, entity);
            return result.Success ? result.Cas : 0;
        }

        public virtual T Get(string id)
        {
            var result = CouchbaseManager.Instance.ExecuteGetJson<T>(id);
            if (result.Success && result.HasValue)
            {
                T doc = result.Value as T;
                if (doc != null)
                {
                    doc.Id = id;
                    doc.CasValue = result.Cas;
                }
                return doc;
            }
            return null;
        }

        public virtual string GetValue(string id)
        {
            var result = CouchbaseManager.Instance.ExecuteGet(id);
            if (result.Success && result.HasValue)
                return result.Value.ToString();
            else
                return null;
        }

        public virtual T GetWithLock(string id)
        {
            var result = CouchbaseManager.Instance.ExecuteGetWithLock(id);
            if (result.Success && result.HasValue)
            {
                T doc = Deserialize(result.Value.ToString());
                if (doc != null)
                {
                    doc.Id = id;
                    doc.CasValue = result.Cas;
                }
                return doc;
            }
            return null;
        }

        public virtual bool Remove(string id)
        {
            return CouchbaseManager.Instance.Remove(id);
        }

        protected virtual IView<IViewRow> View(string viewName)
        {
            return View(typeof(T).Name.ToLower(), viewName);
        }

        protected virtual IView<IViewRow> View(string designDoc, string viewName)
        {
            return CouchbaseManager.Instance.GetView(designDoc, viewName);
        }

        protected virtual ISpatialView<ISpatialViewRow> SpatialView(string viewName)
        {
            return SpatialView(typeof(T).Name.ToLower(), viewName);
        }

        protected virtual ISpatialView<ISpatialViewRow> SpatialView(string designDoc, string viewName)
        {
            return CouchbaseManager.Instance.GetSpatialView(designDoc, viewName);
        }

        public virtual ulong Increment(string key)
        {
            return CouchbaseManager.Instance.Increment(key, 1, 1);
        }

        public virtual T Deserialize(string entity)
        {
            if (!String.IsNullOrEmpty(entity))
                return JsonConvert.DeserializeObject<T>(entity);
            else
                return null;
        }

        public virtual string Serialize(object entity)
        {
            if (entity != null)
                return JsonConvert.SerializeObject(entity);
            else
                return null;
        }

        public virtual Dto<T> GetDto(string viewName,
            StaleMode mode,
            int page = 0,
            int limit = 10,
            int skip = 0,
            bool group = false,
            int groupLevel = 0,
            bool reduce = false,
            string startKey = null,
            string endKey = null,
            string startDocId = null,
            string endDocId = null,
            string sort = null
            )
        {
            if (limit > 1000)
                limit = 1000;

            var view = View(viewName);
            if (limit > 0) view.Limit(limit + 1);
            view.Skip(skip);
            if (!string.IsNullOrEmpty(startKey))
            {
                object[] key = JsonConvert.DeserializeObject<object[]>(startKey.Base64Decode());
                if (key.Length == 1)
                    view.StartKey(key[0]);
                else
                    view.StartKey(key);
            }
            if (!string.IsNullOrEmpty(endKey))
            {
                object[] key = JsonConvert.DeserializeObject<object[]>(endKey.Base64Decode());
                if (key.Length == 1)
                    view.EndKey(key[0]);
                else
                    view.EndKey(key);
            }
            if (!string.IsNullOrEmpty(startDocId)) view.StartDocumentId(startDocId);
            if (!string.IsNullOrEmpty(endDocId)) view.EndDocumentId(endDocId);
            if (group) view.Group(true);
            if (groupLevel > 0) view.GroupAt(groupLevel);
            if (!String.IsNullOrEmpty(sort) && sort.ToLower().Equals("desc")) view.Descending(true);
            view.Reduce(reduce);
            view.Stale(mode);

            Dto<T> dto = new Dto<T>();
            List<T> entites = new List<T>();

            // retrieve results
            List<IViewRow> results = view.ToList();
            if (!results.IsNullOrEmpty())
            {
                int rowCount = results.Count;
                if (rowCount > limit) rowCount = rowCount - 1;
                for (int i = 0; i < rowCount; i++)
                {
                    string docId = results[i].ItemId;
                    if (!String.IsNullOrEmpty(docId))
                    {
                        T url = Get(docId);
                        if (url != null)
                        {
                            url.ViewKey = BuildKey(results[i].ViewKey);
                            entites.Add(url);
                        }
                    }
                }
            }

            dto.Entities = entites;
            dto.TotalRows = view.TotalRows;
            dto.Page = page;
            dto.PageSize = limit;
            dto.Sort = sort;

            if (!results.IsNullOrEmpty())
            {
                dto.SKey = BuildKey(results.FirstOrDefault().ViewKey);
                dto.SId = results.FirstOrDefault().ItemId;
                dto.EKey = BuildKey(results.LastOrDefault().ViewKey);
                dto.EId = results.LastOrDefault().ItemId;
            }

            return dto;
        }

        public static string BuildKey(object[] keys)
        {
            if (keys != null)
                return JsonConvert.SerializeObject(keys).Base64Encode();
            else
                return String.Empty;
        }

        public static object FormatStringKey(string key)
        {
            object[] keys = new object[] { key };
            return keys[0];
        }
    }
}