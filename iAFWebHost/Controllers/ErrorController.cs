using iAFWebHost.Models;
using iAFWebHost.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iAFWebHost.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Errors()
        {
            PageModel model = GetErrors();
            return View("Errors", model);
        }

        public ActionResult Delete()
        {
            if (!String.IsNullOrEmpty(Request["errorid"]))
            {
                if (DeleteError(Request["errorid"]))
                {
                    return RedirectToAction("ErrorMonitor");
                }
            }

            throw new HttpException();
        }

        public ActionResult NotFound()
        {
            return View("_Error");
        }

        public ActionResult Test()
        {
            int i = 0;
            int j = 3;
            var r = j / i;

            return Redirect("/");
        }

        public JsonResult JsonNotFound()
        {
            var response = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    error = true,
                    message = "404 Error. Administrator has been notified"
                }
            };

            return response;
        }

        public JsonResult JsonIndex()
        {
            var response = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    error = true,
                    message = "Error. Administrator has been notified"
                }
            };

            return response;
        }
    }
}