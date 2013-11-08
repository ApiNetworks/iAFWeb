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
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                UrlService urlService = new UrlService();
                var entity = urlService.GetById(id);

                if (!String.IsNullOrEmpty(entity.Href))
                    return Redirect(entity.Href);
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

                                if(!String.IsNullOrEmpty(entity.Href))
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
            {
                ViewBag.Error = "Reason: Invalid shortCode";
                return View("Error");
            }

            AddUser(id, User.Identity.Name);
            return RedirectToAction("UserProfile", "Account", new { username = User.Identity.Name });
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(string id)
        {
            if (!id.IsShortCode())
            {
                ViewBag.Error = "Reason: Invalid shortCode";
                return View("Error");
            }
            
            DeleteUser(id, User.Identity.Name);
            return RedirectToAction("UserProfile", "Account", new { username = User.Identity.Name });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [OutputCache(Duration = 1000)]
        public FileResult Qr(string id, string scale)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int qrScale = 1;
                if (!string.IsNullOrEmpty(scale))
                {
                    Int32.TryParse(scale, out qrScale);
                }

                if (qrScale > 45)
                    qrScale = 45;

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
            }

            return null;
        }

        public ActionResult News()
        {
            PageModel model = new PageModel();
            model.UrlCount = GetUrlCount();
            model.Urls = GetUrlList();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            PageModel model = new PageModel();
            model.UrlCount = GetUrlCount();

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}