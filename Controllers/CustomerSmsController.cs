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
        private OracleDbContext db = new OracleDbContext();

        public class SmsData
        {
            public string Phone;

            public string Password;


            //注册=0，修改资料=1
            public int PhoneStatus;
        }

        
        public class SmsLoginData
        {
            public  string Phone;
            public string ValidateCode;
            public string Password;
        }

        [HttpPost]
        [Route("api/Sms")]
        public IHttpActionResult Sms(SmsData data)
        {
            EBook.Service.SmsSend.SendVerifyCode(data.Phone);


            var result = from customer in db.Customers
                where customer.PhoneNum == data.Phone
                select customer;

            // 电话已存在
            if (result.Any() && data.PhoneStatus == 0)
            {
                return BadRequest("Phone exist");
            }

            EBook.Service.SmsSend.SendVerifyCode(data.Phone);

            return Ok();
        }


        [HttpPost]
        [Route("api/SmsLogin")]
        public IHttpActionResult SmsLogin(SmsLoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = from customer in db.Customers
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
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
                        return BadRequest("Validate code not sent.");
                    case -2:
                        return BadRequest("Wrong validate code.");
                    case -3:
                        return BadRequest("Validate code expired.");
                }
            }


            var updatedCustomer = db.Customers.FirstOrDefault(b => b.PhoneNum == data.Phone);
            if (updatedCustomer != null)
            {
                updatedCustomer.Password = EncryptProvider.Md5(data.Password);
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