using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace iAFWebHost.NLog
{
    // Original Code: http://mvclogging.codeplex.com/releases/view/49755

    [LayoutRenderer("web_variables")]
    public class WebVariablesRenderer : LayoutRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebVariablesRenderer" /> class.
        /// </summary>
        public WebVariablesRenderer()
        {
            this.Format = "";
        }

        /// <summary>
        /// Gets or sets the date format. Can be any argument accepted by DateTime.ToString(format).
        /// </summary>
        /// <docgen category='Rendering Options' order='10' />
        [DefaultParameter]
        public string Format { get; set; }

        /// <summary>
        /// Renders the current date and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);

            writer.WriteStartElement("error");

            // -----------------------------------------
            // Server Variables
            // -----------------------------------------
            writer.WriteStartElement("serverVariables");

            foreach (string key in HttpContext.Current.Request.ServerVariables.AllKeys)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", key);
                writer.WriteStartElement("value");
                writer.WriteAttributeString("string", HttpContext.Current.Request.ServerVariables[key].ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // -----------------------------------------
            // Cookies
            // -----------------------------------------            
            writer.WriteStartElement("cookies");

            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", key);
                writer.WriteStartElement("value");
                writer.WriteAttributeString("string", HttpContext.Current.Request.Cookies[key].Value.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            // -----------------------------------------   

            writer.WriteEndElement();
            // -----------------------------------------   
            writer.Flush();
            writer.Close();
            string xml = sb.ToString();
            builder.Append(xml);
        }

    }
}