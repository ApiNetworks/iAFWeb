using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace iAFWebHost.Helpers
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Return the Current Version from the AssemblyInfo.cs file.
        /// </summary>
        public static string CurrentVersion(this HtmlHelper helper)
        {
            try
            {
                System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return version.Major + "." + version.Minor + "." + version.Build;
            }
            catch (Exception)
            {
                return "?.?.?";
            }
        }
    }
}