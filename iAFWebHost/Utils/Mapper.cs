using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Models;
using iAFWebHost.Entities;

namespace iAFWebHost.Utils
{
    public class Mapper
    {
        public static Url Map(UrlModel model)
        {
            Url entity = new Url();
            entity.Id = model.Id;
            entity.Href = model.Href;
            entity.Flag = model.Flag;
            entity.Title = model.Title;
            entity.Summary = model.Summary;

            if (model.Users != null)
                entity.Users = model.Users;

            return entity;
        }

        public static UrlModel Map(Url entity)
        {
            UrlModel model = new UrlModel();
            model.Id = entity.Id;
            model.Href = entity.Href;
            model.Flag = entity.Flag;
            model.Title = entity.Title;
            model.Summary = entity.Summary;

            if (model.Users != null)
                model.Users = entity.Users;

            return model;
        }
    }
}