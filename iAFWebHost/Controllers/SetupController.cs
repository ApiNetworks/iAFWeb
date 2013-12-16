using Couchbase.Configuration;
using Couchbase.Management;
using Enyim.Caching.Memcached;
using iAFWebHost.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iAFWebHost.Utils;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace iAFWebHost.Controllers
{
    public class SetupController : Controller
    {
        private const string _urlBucket = "urlbucket";
        //
        // GET: /Installer/
        public ActionResult Index()
        {
            CreateBucket();
            return View();
        }

        private void CreateBucket()
        {
            var buckets = ListBuckets();
            foreach (var bucket in buckets)
            {
                if (bucket.Name.Equals(_urlBucket))
                    GetClient().DeleteBucket(_urlBucket);
            }

            Thread.Sleep(2000);

            GetClient().CreateBucket(
                 new Bucket
                 {
                     Name = _urlBucket,
                     AuthType = AuthTypes.Sasl,
                     BucketType = BucketTypes.Membase,
                     Quota = new Quota { RAM = 700 },
                     Password = "iafpassw0rd",
                     ReplicaNumber = ReplicaNumbers.Zero,
                 }
             );

            Thread.Sleep(2000);

            CreateViews();
        }

        private Bucket[] ListBuckets()
        {
            return GetClient().ListBuckets();
        }

        private CouchbaseClientConfiguration GetConfig()
        {
            var config = new CouchbaseClientConfiguration();
            config.Urls.Add(new Uri("http://localhost:8091/pools/"));
            config.Username = "iafadmin";
            config.Password = "!afadmin";
            return config;
        }

        private CouchbaseCluster GetClient()
        {
            var config = GetConfig();
            return new CouchbaseCluster(config);
        }

        private void CreateViews()
        {
            string setupViews = ConfigurationManager.AppSettings["SetupData"];
            if (!String.IsNullOrEmpty(setupViews))
            {
                var files = Directory.GetFiles(System.IO.Path.Combine(setupViews, "views"));
                foreach (string filePath in files)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    string viewName = fileInfo.Name.ToLower().Split('.').First();
                    string viewDoc = ReadDocument(filePath);
                    if (IsValidDesignDocument(viewDoc))
                    {
                        var dev_result = GetClient().CreateDesignDocument(_urlBucket, "dev_" + viewName, viewDoc);
                        var production_result = GetClient().CreateDesignDocument(_urlBucket, viewName, viewDoc);
                    }
                }
            }
        }

        private bool IsValidDesignDocument(string document)
        {
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(document);
            }
            catch (JsonReaderException)
            {
                return false;
            }
            if (jObj["views"] == null) return false;
            return true;
        }

        private string GetTestView()
        {
            var doc = new
            {
                views = new
                {
                    error_list = new
                    {
                        map = "function (doc, meta) { if(doc.t = \"error\" && doc.timeStamp) { emit(dateToArray(doc.timeStamp)); } }",
                        reduce = "_count"
                    },
                }
            };

            return JsonConvert.SerializeObject(doc);
        }

        private string ReadDocument(string filePath)
        {
            string viewDoc;
            using (TextReader reader = new StreamReader(filePath))
            {
                viewDoc = reader.ReadToEnd();
            }
            return string.Join(" ", Regex.Split(viewDoc, @"(?:\r\n|\n|\r)"));
        }
    }
}