using System;
using System.Text;
using System.Web.Helpers;
using System.Security.Cryptography;

namespace EBook.Service
{
    public static class FileUploadAuthorize
    {
        private struct UploadPolicy
        {
            public string scope;
            public long deadline;
        }

        private const string BucketName = "sparkxyf";
        private const string AccessKey = "QGoBQa9YkgBvuExS8bonspckkWTWKaS9KXbVmoV8";
        private const string SecretKey = "ibms3XH0QazGPjuxquvdPPnkvhbxvHVABCoIJxhs";
        
        private static string GenerateUpdatePolicy(string fileName)
        {
            var policy = new UploadPolicy()
            {
                scope = $"{BucketName}:{fileName}",
                deadline = new DateTimeOffset(DateTime.Now.AddMinutes(10)).ToUnixTimeSeconds()
            };

            return Json.Encode(policy);
        }

        private static string WebSafeBase64Encode(string text)
        {
            return WebSafeBase64Encode(Encoding.UTF8.GetBytes(text));
        }

        private static string WebSafeBase64Encode(byte[] data)
        {
            return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_');
        }
        
        public static string GenerateUploadToken(string fileName)
        {
            var policy = GenerateUpdatePolicy(fileName);

            var encodedPolicy = WebSafeBase64Encode(policy);
            // 编码上传策略  得到待编码字符串
        
            var signTool = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey));
            var sign = signTool.ComputeHash(Encoding.UTF8.GetBytes(encodedPolicy));
            // 使用访问密钥签名
        
            var encodedSign = WebSafeBase64Encode(sign);
            // 编码签名
        
            return $"{AccessKey}:{encodedSign}:{encodedPolicy}";
        }
    }
}