using iAF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack.ServiceClient.Web;

namespace iAF
{
    class Program
    {
        static void Main(string[] args)
        {
            //LoadSampleData();
            Demo();
        }

        static void Demo()
        {
            IHttpClient client = new HttpClient();

            string url = "http://www.rambler.ru/";

            Url response = client.Shorten(url);
            Console.WriteLine("Short Url:" + response.ShortHref);
            Console.WriteLine("Short Code:" + response.ShortId);

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
            Console.WriteLine();

            response = client.Expand(response.ShortId);
            Console.WriteLine("Short Code:" + response.ShortId);
            Console.WriteLine("Short Url:" + response.ShortHref);

            Console.WriteLine("Press Any Key to Exit...");
            Console.ReadKey();
        }

        static void LoadSampleData()
        {
            int counter = 0;
            string line;

            IHttpClient client = new HttpClient();

            // Simple data import operation
            using (System.IO.StreamReader file = new System.IO.StreamReader(@"C:\GitHub\iAFWeb\data\top-1m.csv"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    String[] values = line.Split(',');
                    if (values.Length == 2)
                    {
                        string url = String.Format("http://{0}", values[1]);
                        Uri uri;
                        if(Uri.TryCreate(url, UriKind.Absolute, out uri))
                        {
                            var response = client.Shorten(uri.ToString());
                            Console.WriteLine(counter + " : " + response.ShortId + " : " + uri.ToString());
                            counter++;
                        }
                    }
                }
            }

            // Suspend the screen.
            Console.ReadLine();
        }
    }
}
