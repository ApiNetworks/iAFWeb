using iAFWebHost.Entities;
using iAFWebHost.Repositories;
using iAFWebHost.Services;
using iAFWebHost.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAFWebHost.Tests.Repositories
{
    [TestClass]
    public class UrlRepositoryTests
    {
        [TestMethod]
        public void Test_UrlRepository_GetUrlList()
        {
            UrlRepository repo = new UrlRepository();

            // select first page
            Dto<Url> results = repo.GetUrlList();

            // start paging (10 docs per page)
            for (int page = 1; page < 100; page++)
            {
                string lastDocId = results.EId;
                string lastViewKey = results.EKey;
                results = repo.GetUrlList(page, 10, 0, lastViewKey, null, lastDocId, null);
            }

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void Test_UrlRepository_GetDto()
        {
            UrlRepository repo = new UrlRepository();

            int page = 1;
            int pageSize = 10;
            string endKey = null;
            string endDocId = null;

            string endKey2 = null;
            string endDocId2 = null;
            int rowCount = 0;
            do
            {
                Dto<Url> results = repo.GetUrlList(page, pageSize, 0, endKey, null, endDocId, null);
                Assert.IsNotNull(results.Entities);
                rowCount = results.TotalRows - page * pageSize;
                endKey = results.EKey;
                endDocId = results.EId;

                page++;

            } while (rowCount > 0);
        }

        [TestMethod]
        public void Test_Upsert_100()
        {
            UrlRepository repo = new UrlRepository();

            for (int i = 0; i < 100; i++)
            {

                Url entity = new Url();
                entity.Href = String.Format("http://testdomain{0}.com", i);
                entity.Tags.Add("demo");
                entity.Tags.Add("test");
                Url response = repo.Upsert(entity);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(Url));
                Assert.IsNotNull(response.Id);
                Assert.IsNotNull(response.CasValue);

                Url responseCheck = repo.Get(response.Id);
                Assert.IsNotNull(responseCheck);
                Assert.IsInstanceOfType(responseCheck, typeof(Url));
                Assert.IsNotNull(responseCheck.Id);
                Assert.IsNotNull(responseCheck.CasValue);

                Assert.IsTrue(response.Id.Equals(responseCheck.Id));
                Assert.IsTrue(response.Href.Equals(responseCheck.Href));
            }
        }

        [TestMethod]
        public void Test_Upsert_100_SameDomain()
        {
            UrlRepository repo = new UrlRepository();

            for (int i = 0; i < 100; i++)
            {
                Url entity = new Url();
                entity.Href = String.Format("http://demo.com/{0}", i);

                Url response = repo.Upsert(entity);
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(response, typeof(Url));
                Assert.IsNotNull(response.Id);
                Assert.IsNotNull(response.CasValue);

                Url responseCheck = repo.Get(response.Id);
                Assert.IsNotNull(responseCheck);
                Assert.IsInstanceOfType(responseCheck, typeof(Url));
                Assert.IsNotNull(responseCheck.Id);
                Assert.IsNotNull(responseCheck.CasValue);

                Assert.IsTrue(response.Id.Equals(responseCheck.Id));
                Assert.IsTrue(response.Href.Equals(responseCheck.Href));
            }
        }

        [TestMethod]
        public void Test_Upsert_Remove()
        {
            UrlRepository repo = new UrlRepository();

            Url entity = new Url();
            entity.Href = "http://test.com";

            Url response = repo.Upsert(entity);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Id);
            Assert.IsInstanceOfType(response, typeof(Url));

            // remove
            Assert.IsTrue(repo.Remove(response.Id));

            var oldEntity = repo.Get(response.Id);
            Assert.IsNull(oldEntity);
        }

        [TestMethod]
        public void Test_Remove_AllDocuments()
        {
            UrlRepository repo = new UrlRepository();

            // select first page
            Dto<Url> results = repo.GetUrlList();
            int page = 1;
            while (!results.Entities.IsNullOrEmpty())
            {
                foreach (var entity in results.Entities)
                    repo.Remove(entity.Id);

                string lastDocId = results.EId;
                string lastViewKey = results.EKey;

                results = repo.GetUrlList(page, 10, 0, lastViewKey, null, lastDocId, null);
                page++;
            };
        }

        [TestMethod]
        public void Test_Serialize_Deserialize()
        {
            string test = "this_is_a_test";
            object[] o = { test };
            string a = JsonConvert.SerializeObject(o);
            string b = JsonConvert.SerializeObject(o.ToArray());
            string t = a.ToString();
            string t1 = b.ToString();

            Url entity = new Url();
            entity.Href = "http://www.test.com";

            UrlRepository repo = new UrlRepository();
            string json = repo.Serialize(entity);
            Assert.IsNotNull(json);

            Url result = repo.Deserialize(json);
            Assert.IsNotNull(result);
            Assert.IsTrue(entity.Href.Equals(result.Href));
        }

        [TestMethod]
        public void Test_GetUrlCountByUser()
        {
            UrlRepository repo = new UrlRepository();
            int count = repo.GetUrlCountByUser("YuriMenko34");
        }

        [TestMethod]
        public void Test_GetSystemStats()
        {
            UrlRepository repo = new UrlRepository();

            DateTime startDate = new DateTime(2013, 11, 18, 0, 0, 0);
            DateTime endDate = new DateTime(2013, 11, 20, 0, 0, 0);

            List<DataPoint> dataPoints = new List<DataPoint>();

            object[] startKey = { startDate.Year.ToString(), startDate.Month.ToString(), startDate.Day.ToString(), startDate.Hour.ToString() };
            object[] endKey = { endDate.Year.ToString(), endDate.Month.ToString(), endDate.Day.ToString(), endDate.Hour.ToString() };

            var results = repo.GetSystemStats(startKey, endKey, 100, false, 0, true);
        }

        [TestMethod]
        public void Test_GetSystemStatsAggregate()
        {
            UrlRepository repo = new UrlRepository();

            DateTime startDate = new DateTime(2013, 11, 18, 0, 0, 0);
            DateTime endDate = new DateTime(2013, 11, 20, 0, 0, 0);

            List<DataPoint> dataPoints = new List<DataPoint>();

            object[] startKey = { startDate.Year.ToString(), startDate.Month.ToString(), startDate.Day.ToString(), startDate.Hour.ToString() };
            object[] endKey = { endDate.Year.ToString(), endDate.Month.ToString(), endDate.Day.ToString(), endDate.Hour.ToString() };

            StatRecord stats = repo.GetSystemStatsAggregate(startKey, endKey, 100, false, 0);
            Assert.IsNotNull(stats);
        }
    }
}
