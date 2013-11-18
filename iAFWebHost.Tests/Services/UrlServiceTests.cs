using iAFWebHost.Entities;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAFWebHost.Tests.Services
{
    [TestClass]
    public class UrlServiceTests
    {
        [TestMethod]
        public void Test_Single_Upsert()
        {
            UrlService service = new UrlService();

            Url input = GenereateEntity();
            Url output = service.ShortenUrl(input);

            Assert.AreEqual(input.ShortId, String.Empty);
            Assert.IsNotNull(output.ShortId);
        }

        [TestMethod]
        public void Test_Multiple_Upsert()
        {
            UrlService service = new UrlService();

            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            //// Begin timing
            stopwatch.Start();

            Parallel.For(0, 10000, i =>
            {
                Url u = service.ShortenUrl(GenereateEntity());
            });

            // Stop timing
            stopwatch.Stop();

            // Write result
            var res = stopwatch.Elapsed.TotalMilliseconds;
        }

        [TestMethod]
        public void Test_List_Upsert()
        {
            UrlService service = new UrlService();

            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            //// Begin timing
            stopwatch.Start();

            List<Url> list = new List<Url>();
            for (int i = 0; i < 10000; i++)
            {
                list.Add(GenereateUniqueEntity());
            }

            List<Url> response = service.Upsert(list);

            // Stop timing
            stopwatch.Stop();

            // Write result
            var res = stopwatch.Elapsed.TotalMilliseconds;
        }

        [TestMethod()]
        public void Test_Get_Items_By_ShortId()
        {
            UrlService service = new UrlService();

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            Parallel.For(0, 10000, i =>
            {
                Url url = service.ExpandUrl(i.ToString());
            });

            // Stop timing
            stopwatch.Stop();

            // Write result
            string response = stopwatch.Elapsed.TotalMilliseconds.ToString();

            Assert.Inconclusive(response);
        }

        [TestMethod()]
        public void Test_Get_Item_By_ShortId()
        {
            UrlService service = new UrlService();

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            Url url = service.ExpandUrl("M");

            // Stop timing
            stopwatch.Stop();

            // Write result
            string response = stopwatch.Elapsed.TotalMilliseconds.ToString();

            Assert.Inconclusive(response);
        }

        [TestMethod()]
        public void Test_ExceptionHandler()
        {
            BaseService service = new BaseService();
            Assert.Inconclusive();
        }


        [TestMethod()]
        public void Test_Get_Items_By_LongId()
        {
            UrlService service = new UrlService();

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            Parallel.For(1, 10000, i =>
            {
                Url url = service.ExpandUrl(((ulong)i).EncodeBase58());
            });

            // Stop timing
            stopwatch.Stop();

            // Write result
            string response = stopwatch.Elapsed.TotalMilliseconds.ToString();

            Assert.Inconclusive(response);
        }

        [TestMethod()]
        public void Test_Get_Item_By_LongId()
        {
            UrlService service = new UrlService();

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            Url url = service.ExpandUrl(((ulong)45).EncodeBase58());

            // Stop timing
            stopwatch.Stop();

            // Write result
            string response = stopwatch.Elapsed.TotalMilliseconds.ToString();

            Assert.Inconclusive(response);
        }

        [TestMethod()]
        public void Test_GetUrlCount()
        {
            UrlService service = new UrlService();
            int result = service.GetUrlCount();
        }

        [TestMethod()]
        public void Test_GetUrlList()
        {
            UrlService service = new UrlService();
            Dto<Url> results = service.GetUrlList();
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Entities);
            Assert.IsTrue(results.TotalRows > 0);
        }

        private Url GenereateEntity()
        {
            Url entity = new Url();
            entity.Href = "http://www.yahoo.com/";
            return entity;
        }

        private Url GenereateUniqueEntity()
        {
            Url entity = new Url();
            entity.Href = "http://www.yahoo.com/" + Guid.NewGuid().ToString();
            return entity;
        }
    }
}
