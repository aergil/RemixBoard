using System;
using System.IO;
using System.Net;
using System.Text;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public class WebRequestJSON
    {
        public virtual string Get(string uri) {
            try {
                var webRequest = (HttpWebRequest) WebRequest.Create(uri);
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.ContentType = textHtmlCharsetUtf;
                webRequest.Accept = applicationJson;

                var webResponse = webRequest.GetResponse();

                string text;
                using (var sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8)) {
                    text = sr.ReadToEnd();
                }

                return text;
            }
            catch (Exception e) {
                Log.Error(this, "WebRequestJson : Erreur lors de la récupération du flux JSON de " + uri, e);
                return string.Empty;
            }
        }

        private const string textHtmlCharsetUtf = "text/html;charset=UTF-8";
        private const string applicationJson = "application/json";
    }
}