using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;
using System;
using System.Collections.Generic;


namespace EBook.Service
{
    public static class SellerSmsSend
    {
        private static int appid = 1400228523;

        private static  string appkey = "dd155caffdbbc1627cf7b8114517ab6e";

        private static int templateId = 368885;

        private static string smsSign = "";
        
        private struct VerifyRecord
        {
            public  string VerifyCode;
            public  DateTime ValidThrough;

            public VerifyRecord(string verifyCode, DateTime validThrough)
            {
                VerifyCode = verifyCode;
                ValidThrough = validThrough;
            }
        }

        
        private static Dictionary<string, VerifyRecord> _records = new Dictionary<string, VerifyRecord>();

        public  static void SendVerifyCode(string phoneNum)
        {
            var random = new Random();
            var verifyCode = random.Next(0, 9999).ToString("0000");

            if (_records.ContainsKey(phoneNum))
            {
                _records[phoneNum] = new VerifyRecord
                {
                    VerifyCode = verifyCode,
                    ValidThrough = DateTime.Now.AddMinutes(5)
                };
            }
            else
            {
                _records.Add(phoneNum, new VerifyRecord
                {
                    VerifyCode = verifyCode,
                    ValidThrough = DateTime.Now.AddMinutes(5)
                });
            }
            
            
            
            
            try
            {
                SmsSingleSender ssender = new SmsSingleSender(appid, appkey);

                SmsService.Sms(phoneNum,verifyCode);

                var results = ssender.sendWithParam("86", phoneNum,
                    templateId, new[]{verifyCode, "5"}, smsSign, "", "");// 签名参数未提供或者为空时，会使用默认签名发送短信
                Console.WriteLine(results);
            }
            catch (JSONException e)
            {
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
      
        }
        
        public static int CheckVerifyCode(string phone, string code)
        {
            if (_records.ContainsKey(phone))
            {
                var record = _records[phone];
                if (record.VerifyCode != code)
                {
                    return -2;
                }

                if (record.ValidThrough < DateTime.Now)
                {
                    _records.Remove(phone);
                    return -3;
                }

                _records.Remove(phone);
                return 0;
            }
            else
            {
                return -1;
            }
        }
        
        
    }
}