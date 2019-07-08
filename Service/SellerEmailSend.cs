using System;
using System.Collections.Generic;
using Mafly.Mail;

namespace EBook.Service
{
    public static class SellerEmailSend
    {
        private struct VerifyRecord
        {
            public string VerifyCode;
            public DateTime ValidThrough;
        }

        private static readonly Dictionary<string, VerifyRecord> _records = new Dictionary<string, VerifyRecord>();

        public static void SendVerifyCode(string mailAdd)
        {
            var random = new Random();
            var verifyCode = random.Next(0, 999999).ToString("000000");
            if (_records.ContainsKey(mailAdd))
            {
                _records[mailAdd] = new VerifyRecord
                {
                    VerifyCode = verifyCode,
                    ValidThrough = DateTime.Now.AddMinutes(5)
                };
            }
            else
            {
                _records.Add(mailAdd, new VerifyRecord
                {
                    VerifyCode = verifyCode,
                    ValidThrough = DateTime.Now.AddMinutes(5)
                });
            }

            var mailService = new Mail();
            mailService.Send(new MailInfo()
            {
                Receiver = mailAdd,
                Subject = "红杉书屋平台注册验证",
                Body = "您的注册验证码是：" + verifyCode + "。5分钟有效。"
            });
        }

        // :return 0:success  -1: code not set -2:code incorrect -3: Code expired 
        public static int CheckVerifyCode(string mailAdd, string code)
        {
            if (_records.ContainsKey(mailAdd))
            {
                var record = _records[mailAdd];
                if (record.VerifyCode != code)
                {
                    return -2;
                }

                if (record.ValidThrough < DateTime.Now)
                {
                    _records.Remove(mailAdd);
                    return -3;
                }

                _records.Remove(mailAdd);
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}