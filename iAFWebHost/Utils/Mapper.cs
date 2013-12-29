using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Models;
using iAFWebHost.Entities;
using iAF.Entities;

namespace iAFWebHost.Utils
{
    public class Mapper
    {
        public static iAFWebHost.Entities.Url Map(UrlModel model)
        {
            iAFWebHost.Entities.Url entity = new iAFWebHost.Entities.Url();
            entity.Id = model.Id;
            entity.Href = model.Href;
            entity.Flag = model.Flag;
            entity.Title = model.Title;
            entity.Summary = model.Summary;
            entity.UtcDate = model.UtcDate;

            if (model.Users != null)
                entity.Users = model.Users;

            if (model.Tags != null)
                entity.Tags = model.Tags;

            return entity;
        }

        public static UrlModel Map(iAFWebHost.Entities.Url entity)
        {
            UrlModel model = new UrlModel();
            model.Id = entity.Id;
            model.Href = entity.Href;
            model.HrefActual = entity.HrefActual;
            model.Flag = entity.Flag;
            model.Title = entity.Title;
            model.Summary = entity.Summary;
            model.UtcDate = entity.UtcDate;

            if (entity.Users != null)
                model.Users = entity.Users;

            if (entity.Tags != null)
                model.Tags = entity.Tags;

            return model;
        }

        public static iAF.Entities.Url MapResponse(iAFWebHost.Entities.Url entity)
        {
            iAF.Entities.Url model = new iAF.Entities.Url();
            model.Id = entity.Id;
            model.Href = entity.Href;
            model.HrefActual = entity.HrefActual;
            model.Flag = entity.Flag;
            model.Title = entity.Title;
            model.Summary = entity.Summary;
            model.UtcDate = entity.UtcDate;
            model.ShortId = entity.ShortId;
            
            if (entity.Users != null)
                model.Users = entity.Users;

            if (entity.Tags != null)
                model.Tags = entity.Tags;

            return model;
        }

        public static iAFWebHost.Entities.Url MapResponse(iAF.Entities.Url entity)
        {
            iAFWebHost.Entities.Url model = new iAFWebHost.Entities.Url();
            model.Id = entity.Id;
            model.Href = entity.Href;
            model.HrefActual = entity.HrefActual;
            model.Flag = entity.Flag;
            model.Title = entity.Title;
            model.Summary = entity.Summary;
            model.UtcDate = entity.UtcDate;

            if (entity.Users != null)
                model.Users = entity.Users;

            if (entity.Tags != null)
                model.Tags = entity.Tags;

            return model;
        }

        public static iAFWebHost.Models.ErrorModel Map(Entities.Error entity)
        {
            iAFWebHost.Models.ErrorModel model = new ErrorModel();
            model.Id = entity.Id;
            model.FormattedMessage = entity.FormattedMessage;
            model.HasStackTrace = entity.HasStackTrace;
            model.Level = entity.Level;
            model.LoggerName = entity.LoggerName;
            model.Message = entity.Message;
            model.Properties = entity.Properties;
            model.Exception = entity.Exception;
            model.SequenceID = entity.SequenceID;
            model.TimeStamp = entity.TimeStamp;
            model.UserStackFrameNumber = entity.UserStackFrameNumber;
            return model;
        }

        public static DataPointModel Map(DataPoint entity)
        {
            DataPointModel model = new DataPointModel();
            model.ShortId = entity.ShortId;
            model.TimeStamp = entity.UtcTimeStamp;
            model.Value = entity.Value;
            model.Sum = entity.Sum;
            model.SumSqr = entity.SumSqr;
            model.Min = entity.Min;
            model.Max = entity.Max;
            model.Count = entity.Count;
            return model;
        }

        /// <summary>
        /// Maps the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public static PageModel Map(Dto<iAFWebHost.Entities.Url> dto)
        {
            PageModel model = new PageModel();
            List<UrlModel> modelList = new List<UrlModel>();
            foreach (var entity in dto.Entities)
                modelList.Add(Mapper.Map(entity));
            model.Pager.Page = dto.Page;
            model.Pager.PageSize = dto.PageSize;
            model.Pager.TotalRows = dto.TotalRows;
            model.Pager.PreviousKey = dto.SKey;
            model.Pager.PreviousKeyId = dto.SId;
            model.Pager.NextKey = dto.EKey;
            model.Pager.NextKeyId = dto.EId;
            model.Urls = modelList;
            model.UrlCount = dto.TotalRows;
            return model;
        }

        public static PageModel Map(Dto<Error> dto)
        {
            PageModel model = new PageModel();
            List<ErrorModel> modelList = new List<ErrorModel>();
            foreach (var entity in dto.Entities)
                modelList.Add(Mapper.Map(entity));
            model.Pager.Page = dto.Page;
            model.Pager.PageSize = dto.PageSize;
            model.Pager.TotalRows = dto.TotalRows;
            model.Pager.PreviousKey = dto.SKey;
            model.Pager.PreviousKeyId = dto.SId;
            model.Pager.NextKey = dto.EKey;
            model.Pager.NextKeyId = dto.EId;
            model.Errors = modelList;
            return model;
        }
    }
}