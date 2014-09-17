using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArcGISServerGraphAPI.Models
{
// ReSharper disable once InconsistentNaming
    public class ArcGISServerDataProvider
    {
        private static string GetServerUrl()
        {
            return ConfigurationManager.AppSettings["serverUrl"];
        }

        public static JToken GetData(string path)
        {
            var uri = new UriBuilder(new Uri(new Uri(GetServerUrl()), path))
            {
                Query = "f=json"
            }.Uri;

            //var webClient = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            //var data = webClient.DownloadStringTaskAsync(uri);

            var req = (HttpWebRequest)WebRequest.Create(uri);
            req.Credentials = CredentialCache.DefaultCredentials;
            req.ImpersonationLevel = TokenImpersonationLevel.Impersonation;
            using (var serverResponse = req.GetResponse())
            {
                var reader = new StreamReader(serverResponse.GetResponseStream(), Encoding.UTF8);
                var data = reader.ReadToEnd();

                return JsonConvert.DeserializeObject(data) as JToken;
            }
        }
    }
}