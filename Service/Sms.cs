using System.IO;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;


namespace EBook.Service
{
  

public class SmsService
{
    public static String HMACSHA1Text(String EncryptText, String EncryptKey)
    {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);

            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
    }

    public static void Sms(String phone ,String validateCode)
    {
        String url = "https://service-g5r3b98v-1251224662.ap-shanghai.apigateway.myqcloud.com/release/sendsms";
        String method = "GET";
        String querys = "content=您的手机号："+phone+"，验证码："+validateCode+"，请及时完成验证，如不是本人操作请忽略。【腾讯云市场】&mobile="+phone;
        
        
        String source = "market";

        //云市场分配的密钥Id
        String  secretId = "AKIDJSWJ1GJp16tmsStDckjXvN8jqT1miP7kAD0";
        //云市场分配的密钥Key
        String  secretKey = "H17mIF7sYb3NE0UV9X687c1JBTkyCD1uVYo2a310";

        String dt = DateTime.UtcNow.GetDateTimeFormats('r')[0];
        url = url + "?" + querys;

        String signStr = "x-date: " + dt + "\n" + "x-source: " + source;
        String sign = HMACSHA1Text(signStr, secretKey);

        String auth = "hmac id=\"" + secretId + "\", algorithm=\"hmac-sha1\", headers=\"x-date x-source\", signature=\"";
        auth = auth + sign + "\"";
        Console.WriteLine(auth + "\n");

        HttpWebRequest httpRequest = null;
        HttpWebResponse httpResponse = null;

        if (url.Contains("https://"))
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
        }
        else
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
        }

        httpRequest.Method = method;
        httpRequest.Headers.Add("Authorization", auth);
        httpRequest.Headers.Add("X-Source", source);
        httpRequest.Headers.Add("X-Date", dt);

        try
        {
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        }
        catch (WebException ex)
        {
            httpResponse = (HttpWebResponse)ex.Response;
        }
        

    }

    public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }
}
}