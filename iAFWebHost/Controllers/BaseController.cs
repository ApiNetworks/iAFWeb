using iAFWebHost.Entities;
using iAFWebHost.Models;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iAFWebHost.Controllers
{
    public class BaseController : Controller
    {
        private static readonly Lazy<UrlService> _urlService = new Lazy<UrlService>(() => new UrlService());
        private static readonly Lazy<LogService> _logService = new Lazy<LogService>(() => new LogService());

        public UrlService urlService
        {
            get { return _urlService.Value; }
        }

        public LogService logService
        {
            get { return _logService.Value; }
        }

        /// <summary>
        /// Gets the model by identifier.
        /// </summary>
        /// <param name="shortCode">The short code.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">shortCode</exception>
        protected UrlModel GetModelById(string shortCode)
        {
            if (!shortCode.IsShortCode())
                throw new ArgumentNullException("shortCode");

            var entity = GetEntityById(shortCode);
            return Mapper.Map(entity);
        }

        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="shortCode">The short code.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">shortCode</exception>
        protected Url GetEntityById(string shortCode)
        {
            if (!shortCode.IsShortCode())
                throw new ArgumentNullException("shortCode");

            return urlService.ExpandUrl(shortCode);
        }

        /// <summary>
        /// Gets the URL count.
        /// </summary>
        /// <returns></returns>
        protected int GetUrlCount()
        {
            return urlService.GetUrlCount();
        }

        /// <summary>
        /// Gets the URL count by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        protected int GetUrlCountByUser(string userName)
        {
            return urlService.GetUrlCountByUser(userName);
        }

        /// <summary>
        /// Gets the URL count by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        protected int GetUrlCountByTag(string tag)
        {
            return urlService.GetUrlCountByTag(tag);
        }

        /// <summary>
        /// Gets the URL count by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        protected int GetUrlCountByHost(string host)
        {
            return urlService.GetUrlCountByHost(host);
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// Id
        /// or
        /// UserName
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        protected UrlModel AddUser(string id, string userName)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Id");

            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName");

            var entity = urlService.AddUser(id, userName);

            if (entity == null)
                throw new ArgumentException(id);

            return Mapper.Map(entity);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// Id
        /// or
        /// UserName
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        protected UrlModel DeleteUser(string id, string userName)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Id");

            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException("UserName");

            var entity = urlService.DeleteUser(id, userName);

            if (entity == null)
                throw new ArgumentException(id);

            return Mapper.Map(entity);
        }

        /// <summary>
        /// Shortens the URL.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected UrlModel ShortenUrl(UrlModel model)
        {
            Url entity = Mapper.Map(model);
            entity = urlService.ShortenUrl(entity);
            return Mapper.Map(entity);
        }

        /// <summary>
        /// Expands the URL.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        protected UrlModel ExpandUrl(string id)
        {
            UrlModel model = new UrlModel();
            Url entity = urlService.ExpandUrl(id);
            if (entity != null && !String.IsNullOrEmpty(entity.Href))
                model = Mapper.Map(entity);
            return model;
        }

        protected UrlModel ResolveUrl(string id)
        {
            UrlModel model = new UrlModel();
            Url entity = urlService.ExpandUrl(id);
            if (entity != null && entity.Href.IsValidUri())
                UrlServiceHelper.ResolveUrl(entity);

            model = Mapper.Map(entity);
            return model;
        }

        protected List<DataPointModel> GetLast24HourStats(string id)
        {
            var stats = urlService.GetLast24HourStats(id);
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected List<DataPointModel> GetLast30DaysStats(string id)
        {
            var stats = urlService.GetLast30DaysStats(id);
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected List<DataPointModel> GetLast12MonthStats(string id)
        {
            var stats = urlService.GetLast12MonthStats(id);
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected List<DataPointModel> GetLast24HourSystemStats()
        {
            var stats = urlService.GetLast24HourSystemStats();
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected List<DataPointModel> GetLast30DaysSystemStats()
        {
            var stats = urlService.GetLast30DaysSystemStats();
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected List<DataPointModel> GetLast12MonthSystemStats()
        {
            var stats = urlService.GetLast12MonthSystemStats();
            List<DataPointModel> statsModels = new List<DataPointModel>();
            foreach (var stat in stats)
            {
                var m = Mapper.Map(stat);
                statsModels.Add(m);
            }
            return statsModels;
        }

        protected void IncrementHitCount(string id)
        {
            urlService.IncrementHitCount(id);
        }

        /// <summary>
        /// Gets the URL list.
        /// </summary>
        /// <returns></returns>
        protected PageModel GetUrlList()
        {
            PageHelper pageHelper = ParsePageHelper();
            pageHelper.PageSize = 100;
            Dto<Url> dto = urlService.GetUrlList(pageHelper.Page, pageHelper.PageSize, pageHelper.Skip, pageHelper.NextKey, null, pageHelper.NextKeyId, null, pageHelper.Sort);
            PageModel model = Mapper.Map(dto);
            model.Pager.TempKey = pageHelper.PreviousKey;
            model.Pager.TempKeyId = pageHelper.PreviousKeyId;
            return model;
        }

        /// <summary>
        /// Gets the URL list by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        protected PageModel GetUrlListByUser(string userName)
        {
            PageHelper pageHelper = ParsePageHelper();
            Dto<Url> dto = urlService.GetUrlListByUser(pageHelper.Page, pageHelper.PageSize, pageHelper.Skip, userName, userName, pageHelper.NextKeyId, null, pageHelper.Sort);
            PageModel model = Mapper.Map(dto);
            model.Pager.TempKey = pageHelper.PreviousKey;
            model.Pager.TempKeyId = pageHelper.PreviousKeyId;
            model.Pager.TotalRows = urlService.GetUrlCountByUser(userName);
            return model;
        }

        /// <summary>
        /// Gets the URL list by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        protected PageModel GetUrlListByHost(string host)
        {
            PageHelper pageHelper = ParsePageHelper();
            Dto<Url> dto = urlService.GetUrlListByHost(pageHelper.Page, pageHelper.PageSize, pageHelper.Skip, host, host, pageHelper.NextKeyId, null, pageHelper.Sort);
            PageModel model = Mapper.Map(dto);
            model.Pager.TempKey = pageHelper.PreviousKey;
            model.Pager.TempKeyId = pageHelper.PreviousKeyId;
            model.Pager.TotalRows = urlService.GetUrlCountByHost(host);
            return model;
        }

        /// <summary>
        /// Gets the URL list by tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        protected PageModel GetUrlListByTag(string tagName)
        {
            PageHelper pageHelper = ParsePageHelper();
            Dto<Url> dto = urlService.GetUrlListByTag(pageHelper.Page, pageHelper.PageSize, pageHelper.Skip, tagName, tagName, pageHelper.NextKeyId, null, pageHelper.Sort);
            PageModel model = Mapper.Map(dto);
            model.Pager.TempKey = pageHelper.PreviousKey;
            model.Pager.TempKeyId = pageHelper.PreviousKeyId;
            model.Pager.TotalRows = urlService.GetUrlCountByTag(tagName);
            return model;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <returns></returns>
        protected PageModel GetErrors()
        {
            PageHelper pageHelper = ParsePageHelper();
            Dto<Error> dto = logService.GetErrors();
            PageModel model = Mapper.Map(dto);
            model.Pager.TempKey = pageHelper.PreviousKey;
            model.Pager.TempKeyId = pageHelper.PreviousKeyId;
            return model;
        }

        protected bool DeleteError(string id)
        {
            return logService.DeleteError(id);
        }

        /// <summary>
        /// Parses the page helper.
        /// </summary>
        /// <returns></returns>
        protected PageHelper ParsePageHelper()
        {
            PageHelper pageHelper = new PageHelper();
            pageHelper.Page = 1;
            pageHelper.PageSize = 10;
            if (!String.IsNullOrEmpty(Request["p"])
                && !String.IsNullOrEmpty(Request["ps"])
                && !String.IsNullOrEmpty(Request["nk"])
                && !String.IsNullOrEmpty(Request["nid"]))
            {
                int page = 1;
                if (Int32.TryParse(Request["p"], out page))
                    pageHelper.Page = page;
                int pageSize = 10;
                if (Int32.TryParse(Request["ps"], out pageSize))
                {
                    if (pageSize > 100) pageSize = 100;
                    pageHelper.PageSize = pageSize;
                }
                int skip = 0;
                if (!String.IsNullOrEmpty(Request["skip"]) && Int32.TryParse(Request["skip"], out skip))
                {
                    if (skip > 1) skip = 1;
                    pageHelper.Skip = skip;
                }
                pageHelper.NextKey = Request["nk"];
                pageHelper.NextKeyId = Request["nid"];
                pageHelper.PreviousKey = Request["pk"];
                pageHelper.PreviousKeyId = Request["pid"];
                if (!String.IsNullOrEmpty(Request["sort"]))
                {
                    if (Request["sort"].ToLower().Equals("desc")) ;
                    pageHelper.Sort = "desc";
                }
            }
            return pageHelper;
        }
    }
}