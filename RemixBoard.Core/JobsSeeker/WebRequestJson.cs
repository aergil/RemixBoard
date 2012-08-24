using System;
using System.IO;
using System.Net;
using System.Text;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public class WebRequestJson
    {
        public virtual string Get(string uri) {
            try {
                var webRequest = (HttpWebRequest) WebRequest.Create(uri);
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.ContentType = textHtmlCharsetUtf;
                webRequest.Accept = applicationJson;

                var webResponse = webRequest.GetResponse();

                string fluxJson;
                Stream stream = webResponse.GetResponseStream();
                if(stream == null)
                    throw new Exception("La réponse de la requète est nulle");

                using (var sr = new StreamReader(stream, Encoding.UTF8)) {
                    fluxJson = sr.ReadToEnd();
                }

                return fluxJson;
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