using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iAFWebHost.Entities;
using iAFWebHost.Services;
using iAFWebHost.Models;
using iAFWebHost.Utils;

namespace iAFWebHost.Controllers
{
    public class UrlController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            if (id.IsShortCode())
            {
                var model = ExpandUrl(id);
                if (model != null && !String.IsNullOrEmpty(model.Href))
                {
                    IncrementHitCount(id);
                    LogHttpRequestAsync();

                    return Redirect(model.Href);
                }
                else
                    throw new HttpException();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(UrlModel model)
        {
            // Only operate on valid models
            if (ModelState.IsValid)
            {
                // Model must have a valid Href attribute
                if (!String.IsNullOrEmpty(model.Href))
                {
                    // if host is equals i.af it means a redirection is attempted
                    if (model.Host.Equals("i.af", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // try and extract shortId from the url
                        string[] urlArray = model.Href.Split('/');
                        if (urlArray.Length > 0)
                        {
                            string shortCode = urlArray[urlArray.Length - 1];
                            if (!String.IsNullOrEmpty(shortCode))
                            {
                                var entity = GetEntityById(shortCode);

                                if (!String.IsNullOrEmpty(entity.Href))
                                    return Redirect(entity.Href);
                            }
                        }
                    }
                    else
                    {
                        // Add user attribute if user is authenticated
                        if (User.Identity.IsAuthenticated)
                        {
                            model.Users.Add(User.Identity.Name);
                        }

                        model = ShortenUrl(model);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddUser(string id)
        {
            if (!id.IsShortCode())
                throw new HttpException(404, "Not Found");

            AddUser(id, User.Identity.Name);
            return RedirectToAction("UserProfile", "Account", new { username = User.Identity.Name });
        }

        public ActionResult Ad(string id)
        {
            if (id.IsShortCode())
            {
                UrlService urlService = new UrlService();
                Url entity = urlService.ExpandUrl(id);
                if (entity != null && !String.IsNullOrEmpty(entity.Href))
                {
                    UrlPageModel model = new UrlPageModel();
                    model.UrlModel = Mapper.Map(entity);
                    return View(model);
                }
                else
                    throw new HttpException();
            }

            return View();
        }

        public ActionResult Banner(string id)
        {
            var t = Request.QueryString;
            return Redirect("http://www.yahoo.com");
        }

        public ActionResult Stats(string id)
        {
            if (id.IsShortCode())
            {
                // display individual url stats
                UrlModel urlModel = ExpandUrl(id);
                if (urlModel != null && !String.IsNullOrEmpty(urlModel.Href))
                {
                    UrlPageModel model = new UrlPageModel();
                    model.UrlModel = urlModel;
                    model.HourlyDataPoints = GetLast24HourStats(id);
                    model.DailyDataPoints = GetLast30DaysStats(id);
                    model.MonthlyDataPoints = GetLast12MonthStats(id);
                    return View(model);
                }
            }
            else
            {
                //display global system stats
                UrlPageModel model = new UrlPageModel();
                model.HourlyDataPoints = GetLast24HourSystemStats();
                model.DailyDataPoints = GetLast30DaysSystemStats();
                model.MonthlyDataPoints = GetLast12MonthSystemStats();
                return View("SystemStats", model);
            }

            return RedirectPermanent("/");
        }

        public ActionResult Requests()
        {
            UrlPageModel model = GetRequests();
            return View(model);
        }

        public ActionResult Referrals()
        {
            UrlPageModel model = GetRequestsWithReferrals();
            return View(model);
        }

        public ActionResult Trace(string id)
        {
            if (id.IsShortCode())
            {
                // display individual url stats
                UrlModel urlModel = ExpandUrl(id);

                // Attempt to resolve url on demand
                if (String.IsNullOrEmpty(urlModel.HrefActual))
                    urlModel = ResolveUrl(id);
                
                if (urlModel != null && urlModel.Href.IsValidUri())
                {
                    UrlPageModel model = new UrlPageModel();
                    model.UrlModel = urlModel;
                    return View(model);
                }
            }

            return RedirectPermanent("/");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(string id)
        {
            if (!id.IsShortCode())
                throw new HttpException(404, "Not Found");

            DeleteUser(id, User.Identity.Name);
            return RedirectToAction("UserProfile", "Account", new { username = User.Identity.Name });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [OutputCache(Duration = 1000)]
        public FileResult Qr(string id, string scale)
        {
            if (!id.IsShortCode())
                throw new HttpException(404, "Not Found");

            int qrScale = 1;
            if (!string.IsNullOrEmpty(scale))
                Int32.TryParse(scale, out qrScale);

            if (qrScale > 45) qrScale = 45;

            Url entity = base.GetEntityById(id);
            if (entity != null)
            {
                var img = QRGenerator.GenerateImage(entity.ShortHref, qrScale);
                if (img != null)
                {
                    ImageResult image = new ImageResult(img);
                    return image;
                }
            }

            return null;
        }

        public ActionResult Site(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            UrlPageModel model = GetUrlListByHost(id);
            model.UrlCount = GetUrlCountByHost(id);
            model.Host = id;

            return View(model);
        }

        public ActionResult Tag(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            UrlPageModel model = GetUrlListByTag(id);
            model.UrlCount = GetUrlCountByTag(id);
            model.Tag = id;
            return View(model);
        }

        public ActionResult News()
        {
            UrlPageModel model = GetUrlList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            UrlPageModel model = new UrlPageModel();
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}