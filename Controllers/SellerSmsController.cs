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
            public  string Phone;

            public string Password;
            //注册=0，修改资料=1
            public int PhoneStatus;
        }


        public class SellerSmsLoginData
        {
            public  string Phone;
            public string ValidateCode;
            public string NewPassword;
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
                return BadRequest("Phone exist");
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
                }
            }

      
            var updatedSeller = db.Sellers.FirstOrDefault(b => b.SellerPhone == data.Phone);
            if (updatedSeller != null)
            {
                updatedSeller.Password = EncryptProvider.Md5(data.NewPassword);
                db.SaveChanges();
                return Ok("Update Success");
            }
            else
            {
                return NotFound();
            }
            
        }
        
        
    }
}