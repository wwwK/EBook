using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using EBook.Models;
using EBook.Service;
using NETCore.Encrypt;
using OracleInternal.Secure.Network;

namespace EBook.Controllers
{
    public class SellerSmsController:ApiController
    {
        
        private OracleDbContext db = new OracleDbContext();
        public class SmsData
        {
            public readonly string Phone;
            
            //注册=0，修改资料=1
            public readonly int PhoneStatus;

            public SmsData(string phone, int phoneStatus)
            {
                Phone = phone;
                PhoneStatus = phoneStatus;
            }
        }


        public class SellerSmsLoginData
        {
            public readonly string Phone;
            public readonly string ValidateCode;
            public readonly string Password;

            public SellerSmsLoginData(string phone, string validateCode, string password)
            {
                Phone = phone;
                ValidateCode = validateCode;
                Password = password;
            }
        }
        
        
        
        
        //用于注册对时候使用为0，重置密码或者验证码登陆为1
        [HttpPost]
        [Route("api/SellerSms")]
        public IHttpActionResult SellerSms(SmsData data)
        {
            
            var result = from seller in db.Sellers
                where seller.SellerPhone == data.Phone
                select seller;

            // 电话已存在
            if (result.Any() && data.PhoneStatus == 0)
            {
                return BadRequest("电话已经被注册了，请输入新的电话号码！");
            }

            
            EBook.Service.SellerSmsSend.SendVerifyCode(data.Phone);
            
            return Ok();
        }

        [HttpPost]
        [Route("api/SellerSmsLogin")]
        public IHttpActionResult SellerSmsLogin(SellerSmsLoginData data)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = from seller in db.Sellers
                where seller.SellerPhone == data.Phone
                select seller;

            if (!result.Any())
            {

                return NotFound();
            }
      
            
            
            var tmpResult = Service.SellerSmsSend.CheckVerifyCode(data.Phone, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("请先点击发送验证码！");
                    case -2:
                        return BadRequest("验证码错误，请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新发送验证码！");
                }
            }
            
            
            var cookie = new HttpCookie("sessionId")
            {
                Value = SellerSession.SetSessionId(result.First().SellerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };
            
            HttpContext.Current.Response.Cookies.Add(cookie);
            return Ok();
        }



        [HttpPost]
        [Route("api/SellerSmsChangePassword")]
        public IHttpActionResult SellerSmsChangePassword(SellerSmsLoginData data)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var tmpResult = Service.SellerSmsSend.CheckVerifyCode(data.Phone, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("请先点击发送验证码！");
                    case -2:
                        return BadRequest("验证码错误，请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新发送验证码！");
                }
            }

      
            var updatedSeller = db.Sellers.FirstOrDefault(b => b.SellerPhone == data.Phone);
            if (updatedSeller != null)
            {
                updatedSeller.Password = EncryptProvider.Md5(data.Password);
                db.SaveChanges();
                return Ok("重置密码成功！");
            }
            else
            {
                return NotFound();
            }
            
        }
        
        
    }
}