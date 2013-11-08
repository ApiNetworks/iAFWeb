using iAFWebHost.Models;
using iAFWebHost.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace iAFWebHost.Controllers
{
    [Route("api/v1/url/{id}")]
    public class ApiUrlController : ApiController
    {
        public UrlModel Get(string id)
        {
            UrlService urlService = new UrlService();
            var entity = urlService.GetById(id);
            return Utils.Mapper.Map(entity);
        }

        // POST api/api
        public void Post([FromBody]string value)
        {
        }

        // PUT api/api/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/api/5
        public void Delete(int id)
        {

        }
    }
}
