using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAFWebHost.Repositories;

namespace iAFWebHost.Tests.Repositories
{
    [TestClass]
    public class EmailRepositoryTests
    {
        [TestMethod]
        public void EmailRepository_Test_EmailInstance()
        {
            EmailRepository repo = new EmailRepository();
            Couchbase.CouchbaseClient instance = CouchbaseManager.EmailInstance;
            Assert.IsInstanceOfType(instance, typeof(Couchbase.CouchbaseClient));
            
        }
    }
}
