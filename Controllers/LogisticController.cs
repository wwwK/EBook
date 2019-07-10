using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;

namespace EBook.Controllers
{
    public class LogisticController:ApiController
    {
        
        
        public class LogisticData
        {
            public readonly string LogisticId;

            public LogisticData(string logisticId)
            {
                this.LogisticId = logisticId;
            }
        }
        
        
        private const String Host = "https://wuliu.market.alicloudapi.com";
        private const String Path = "/kdi";
        private const String Method = "GET";
        private const String AppCode = "b97dd3ef6e404289b123ab1f3a548209";
        
        
        [HttpPost]
        [Route("api/Logistic")]
        public IHttpActionResult LogisticSearch(LogisticData logisticData)
        {
            String querys = "no=" + logisticData.LogisticId;
            String bodys = "";
            String url = Host + Path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (Host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = Method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + AppCode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));


            
            return Ok(reader.ReadToEnd());
        }
        
        
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}