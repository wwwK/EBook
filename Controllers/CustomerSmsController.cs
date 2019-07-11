using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using EBook.Models;
using EBook.Service;
using NETCore.Encrypt;

namespace EBook.Controllers
{
    public class CustomerSmsController : ApiController
    {
        private readonly OracleDbContext _db = new OracleDbContext();

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

        
        public class SmsLoginData
        {
            public readonly string Phone;
            public readonly string ValidateCode;
            public readonly string Password;

            public SmsLoginData(string phone, string validateCode, string password)
            {
                Phone = phone;
                ValidateCode = validateCode;
                Password = password;
            }
        }

        [HttpPost]
        [Route("api/Sms")]
        public IHttpActionResult Sms(SmsData data)
        {
            
            var result = from customer in _db.Customers
                where customer.PhoneNum == data.Phone
                select customer;

            // 电话已存在
            if (result.Any() && data.PhoneStatus == 0)
            {
                return BadRequest("您输入的电话已经被注册了！");
            }

            EBook.Service.SmsSend.SendVerifyCode(data.Phone);

            return Ok("验证码已经发送！");
        }


        [HttpPost]
        [Route("api/SmsLogin")]
        public IHttpActionResult SmsLogin(SmsLoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = from customer in _db.Customers
                where customer.PhoneNum == data.Phone
                select customer;

            if (!result.Any())
            {
                return NotFound();
            }


            var tmpResult = Service.SmsSend.CheckVerifyCode(data.Phone, data.ValidateCode);
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
                Value = SellerSession.SetSessionId(result.First().CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
            return Ok();
        }


        [HttpPost]
        [Route("api/SmsChangePassword")]
        public IHttpActionResult SmsChangePassword(SmsLoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var tmpResult = Service.SmsSend.CheckVerifyCode(data.Phone, data.ValidateCode);
            if (tmpResult != 0)
            {
                switch (tmpResult)
                {
                    case -1:
                        return BadRequest("V请先点击发送验证啊吗！");
                    case -2:
                        return BadRequest("验证码错误，请输入正确的验证码！");
                    case -3:
                        return BadRequest("请重新发送验证码！");
                }
            }


            var updatedCustomer = _db.Customers.FirstOrDefault(b => b.PhoneNum == data.Phone);
            if (updatedCustomer != null)
            {
                updatedCustomer.Password = EncryptProvider.Md5(data.Password);
                _db.SaveChanges();
                return Ok("重置密码成功！");
            }
            else
            {
                return NotFound();
            }
        }
    }
}