using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using EBook.Models;
using BCrypt.Net;
using EBook.Service;

namespace EBook.Controllers
{
    public class AuthController : ApiController
    {
        private OracleDbContext db = new OracleDbContext();


        public class LoginData
        {
            [EmailAddress] public readonly string Email;

            public readonly string Password;


            //注册=0，修改资料=1
            public readonly int EmailStatus;

            public LoginData(string email, string password, int emailStatus)
            {
                Email = email;
                Password = password;
                EmailStatus = emailStatus;
            }
        }
        public class SmsData
        {
            public  string Phone;

            public string Password;


            //注册=0，修改资料=1
            public int PhoneStatus;

        
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
        [Route("api/SendMail")]
        public IHttpActionResult SendMail(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email Invalid.");
            }

            var result = from customer in db.Customers
                where customer.Email == data.Email
                select customer;

            // 邮箱已存在
            if (result.Any() && data.EmailStatus == 0)
            {
                return BadRequest("Email exist");
            }

            EBook.Service.EmailSend.SendVerifyCode(data.Email);
            return Ok();
        }

        [HttpPost]
        [Route("api/Login")]
        public IHttpActionResult Login(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = from customer in db.Customers
                where customer.Email == data.Email
                select customer;

            if (!result.Any())
            {
                return NotFound();
            }

            var hashed = BCrypt.Net.BCrypt.HashPassword(data.Password);
            if (result.First().Password != hashed)
            {
                return BadRequest("Password incorrect.");
            }

            var cookie = new HttpCookie("sessionId")
            {
                Value = CustomerSession.SetSessionId(result.First().CustomerId).ToString(),
                Expires = DateTime.Now.AddHours(1)
            };


            HttpContext.Current.Response.Cookies.Add(cookie);


            return Ok();
        }


        [HttpPost]
        [Route("api/Logout")]
        public IHttpActionResult Logout()
        {
            var session = HttpContext.Current.Request.Cookies.Get("sessionId");
            if (session == null)
            {
                return Ok();
            }

            CustomerSession.RemoveCustomerIdFromSession(int.Parse(session.Value));
            HttpContext.Current.Response.Cookies.Remove("sessionId");

            return Ok();
        }
    }
}