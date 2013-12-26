using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iAFWebHost.Utils;

namespace iAFWebHost.Tests.Extensions
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void Test_IsValidUri()
        {
            string invalidUrl = String.Empty;
            Assert.IsFalse(invalidUrl.IsValidUri());
            
            invalidUrl = null;
            Assert.IsFalse(invalidUrl.IsValidUri());

            invalidUrl = "http";
            Assert.IsFalse(invalidUrl.IsValidUri());

            string validUrl = "http://www.google.com";
            Assert.IsTrue(validUrl.IsValidUri());
        }
    }
}
