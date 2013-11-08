using iAFWebHost.Entities;
using iAFWebHost.Models;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iAFWebHost.Controllers
{
    public class BaseController : Controller
    {
        private UrlService urlService; 

        public BaseController()
        {
            urlService = new UrlService();
        }

        protected UrlModel GetModelById(string shortCode)
        {
            if (!shortCode.IsShortCode())
                throw new ArgumentNullException("shortCode");

            var entity = GetEntityById(shortCode);
            return Mapper.Map(entity);
        }

        protected Url GetEntityById(string shortCode)
        {
            if (!shortCode.IsShortCode())
                throw new ArgumentNullException("shortCode");

            return urlService.GetById(shortCode);
        }

        protected int GetUrlCount()
        {
            return urlService.GetUrlCount();
        }

        protected int GetUserUrlCount(string userName)
        {
            return urlService.GetUserUrlCount(userName);
        }

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

        protected UrlModel ShortenUrl(UrlModel model)
        {
            Url entity = Mapper.Map(model);
            entity = urlService.ShortenUrl(entity);
            return Mapper.Map(entity);
        }

        protected List<UrlModel> GetUrlList()
        {
            var entityList = urlService.GetUrlList();

            List<UrlModel> modelList = new List<UrlModel>();
            foreach (var entity in entityList)
            {
                modelList.Add(Mapper.Map(entity));
            }
            return modelList;
        }

        protected List<UrlModel> GetUserUrlList(string userName)
        {
            var entityList = urlService.GetUserUrlList(userName);

            List<UrlModel> modelList = new List<UrlModel>();
            foreach (var entity in entityList)
            {
                modelList.Add(Mapper.Map(entity));
            }
            return modelList;
        }
	}
}